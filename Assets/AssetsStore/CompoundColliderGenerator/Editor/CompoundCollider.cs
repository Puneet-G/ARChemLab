//	=============================
//	 Compound Collider Generator 
//	=============================
//	Version 1.1
//  Copyright (c) 2016 Kyle Vekos
//	Unity Asset Store Page: http://u3d.as/tjD
//	=============================

using UnityEngine;
using UnityEditor;
using ProceduralToolkit;

namespace CompoundColliderGenerator
{

	#region Enums

	public enum CompoundShape
	{
		Ring,
		Box,
		Bowl
	};

	public enum ColliderShape
	{
		Box,
		Capsule
	};

	public enum PivotAxis
	{
		X,
		Y,
		Z
	};

	#endregion

	public static class CompoundCollider
	{
		private static readonly Material colliderMaterial =
			AssetDatabase.LoadAssetAtPath<Material>(
				"Assets/CompoundColliderGenerator/Editor/Resources/Materials/ColliderMaterial.mat");

		/// <summary>
		/// Creates a ring-shaped compound collider game object.
		/// </summary>
		public static GameObject CreateRing(PivotAxis pivotAxis, int sides, float outerRadius, float innerRadius,
			ColliderShape colliderShape, float height, float rotationOffset, bool hasEndCap)
		{
			// Empty game object that will serve as the root of the compound collider "prefab"
			var compoundCollider = new GameObject("Ring Compound Collider");

			float length = 2*outerRadius*Mathf.Tan(Mathf.PI/sides); // Length follows the flow of the ring of the torus
			float width = outerRadius - innerRadius;

			if (colliderShape.Equals(ColliderShape.Capsule))
			{
				// Capsule length needs to be adjusted to factor in spherical caps
				length = (length / outerRadius * ((outerRadius + innerRadius) / 2)) + (width);
			}

			for (int i = 0; i < sides; i++)
			{
				// Create the new collider object
				GameObject segment = null;

				if (colliderShape.Equals(ColliderShape.Box))
				{
					segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
					segment.transform.localScale = new Vector3(length, height, width); // Set scale
					segment.transform.Rotate(Vector3.right, rotationOffset); // Apply lengthwise rotation

					// Set the renderer material on the segment object
					segment.GetComponent<Renderer>().material = colliderMaterial;
				}
				else if (colliderShape.Equals(ColliderShape.Capsule))
				{
					// Create segment game object and set its position
					segment = new GameObject("Capsule");

					// Add CapsuleCollider component
					var capsuleCollider = segment.AddComponent<CapsuleCollider>();

					// Turn capsule so it lies length-wise along the X-axis
					capsuleCollider.direction = 0;

					capsuleCollider.height = length;
					capsuleCollider.radius = width / 2;
				}				

				float segmentAngleDegrees = i*(360f/sides);

				// Position the segment relative to its parent
				segment.transform.position = new Vector3(0f, 0f, outerRadius - (width/2));
				segment.transform.RotateAround(Vector3.zero, Vector3.up, segmentAngleDegrees);

				// Rotate to matcb pivot axis
				if (pivotAxis != PivotAxis.Y) // We do Y by default so skip this step if Y
				{
					Vector3 rotationAxis = (pivotAxis == PivotAxis.Z) ? Vector3.right : Vector3.forward;
					segment.transform.RotateAround(Vector3.zero, rotationAxis, 90f);
				}

				// Set the segment parent to the compound collider GameObject
				segment.transform.SetParent(compoundCollider.transform, true);
			}

			// Add the cap if enabled
			if (hasEndCap)
			{
				var capObject = CreateCylinderPrimitive(sides, outerRadius, width);

				// Position the cap on the bottom of the tube
				capObject.transform.position = new Vector3(0f, -((height/2) + width/2), 0f);

				// Set the cap's parent to the compound collider GameObject
				capObject.transform.SetParent(compoundCollider.transform, true);

				// Set the renderer material on the segment object
				capObject.GetComponent<Renderer>().material = colliderMaterial;
			}

			// Register undo event for creation of compound collider
			Undo.RegisterCreatedObjectUndo(compoundCollider, "Create Torus Compound Collider");

			return compoundCollider;
		}

		/// <summary>
		/// Creates an open-top, box-shaped compound collider game object.
		/// </summary>
		public static GameObject CreateBox(float length, float width, float height, float wallThickness)
		{
			// Empty game object that will serve as the root of the compound collider "prefab"
			var compoundCollider = new GameObject("Box Compound Collider");

			// Create bottom collider
			var bottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
			bottom.name = "Bottom";
			bottom.transform.localScale = new Vector3(width, wallThickness, length);
			bottom.transform.position = new Vector3(0f, -((height/2) - (wallThickness/2)), 0f);
			bottom.transform.SetParent(compoundCollider.transform);
			bottom.GetComponent<Renderer>().material = colliderMaterial;

			// Create width-wise (X-axis parallel) walls
			var front = GameObject.CreatePrimitive(PrimitiveType.Cube);
			front.transform.localScale = new Vector3(width, height - wallThickness, wallThickness);
			front.transform.position = new Vector3(0f, wallThickness/2, (length/2) - (wallThickness/2));
			front.transform.SetParent(compoundCollider.transform);
			front.GetComponent<Renderer>().material = colliderMaterial;

			var back = GameObject.CreatePrimitive(PrimitiveType.Cube);
			back.transform.localScale = new Vector3(width, height - wallThickness, wallThickness);
			back.transform.position = new Vector3(0f, wallThickness/2, -((length/2) - (wallThickness/2)));
			back.transform.SetParent(compoundCollider.transform);
			back.GetComponent<Renderer>().material = colliderMaterial;

			// Create length-wise (Z-axis parallel) walls
			var right = GameObject.CreatePrimitive(PrimitiveType.Cube);
			right.transform.localScale = new Vector3(wallThickness, height - wallThickness, length - (wallThickness*2));
			right.transform.position = new Vector3((width/2) - (wallThickness/2), wallThickness/2, 0f);
			right.transform.SetParent(compoundCollider.transform);
			right.GetComponent<Renderer>().material = colliderMaterial;

			var left = GameObject.CreatePrimitive(PrimitiveType.Cube);
			left.transform.localScale = new Vector3(wallThickness, height - wallThickness, length - (wallThickness*2));
			left.transform.position = new Vector3(-((width/2) - (wallThickness/2)), wallThickness/2, 0f);
			left.transform.SetParent(compoundCollider.transform);
			left.GetComponent<Renderer>().material = colliderMaterial;

			// Register undo event for creation of compound collider
			Undo.RegisterCreatedObjectUndo(compoundCollider, "Create Box Compound Collider");

			return compoundCollider;
		}

		/// <summary>
		/// Creates bowl-shaped compound collider game object.
		/// </summary>
		public static GameObject CreateBowl(float radius, int verticalSegments, float thickness, float range)
		{
			// Empty game object that will serve as the root of the compound collider "prefab"
			var compoundCollider = new GameObject("Bowl Compound Collider");

			// Start with a full ring shape
			var bowlSlice = 
				CreateRing(PivotAxis.Z, verticalSegments, radius, radius - thickness, ColliderShape.Capsule, thickness, 0f, false);
			bowlSlice.name = "Bowl Slice";

			// Calculate length of a full slice segment
			float segmentLength = 2 * radius * Mathf.Tan(Mathf.PI / verticalSegments);
			// Capsule segment length needs to be adjusted to factor in spherical caps
			segmentLength = (segmentLength / radius * ((radius + (radius - thickness)) / 2)) + (thickness);

			// Calculate slice length
			float sliceLength = (segmentLength * verticalSegments) * (range / 360f);

			// Destroy the extra ring segments
			int sliceSegmentCount = (int)Mathf.Ceil(sliceLength / segmentLength / 2f);
			for (int i = verticalSegments - 1; i > sliceSegmentCount - 1; i--)
			{
				GameObject.DestroyImmediate(bowlSlice.transform.GetChild(i).gameObject);
			}

			// Resize and reposition end cap segment
			var endCapSegment = bowlSlice.transform.GetChild(sliceSegmentCount - 1).gameObject;
			float endCapLength = (sliceLength / 2f) - (segmentLength * (sliceSegmentCount - 1));

			// Since capsule can't be shorter than its diameter we will delete end cap if its length is too short
			if (endCapLength < thickness)
			{
				GameObject.DestroyImmediate(endCapSegment);
			}
			else
			{
				// Set the end cap's length
				endCapSegment.GetComponent<CapsuleCollider>().height = endCapLength;

				// Slide the segment along its length back towards the preceding segment
				endCapSegment.transform.Translate(-((segmentLength - endCapLength) / 2f), 0f, 0f, Space.Self);
			}			

			// Calculate the angle between current and previous segments relative to bowl center
			float segmentAngle = 360f / verticalSegments;

			// Rotate the slice along the Z-axis so the bottom of the bowl comes to a point
			bowlSlice.transform.RotateAround(Vector3.zero, Vector3.forward, segmentAngle/2f);

			// Find the angle between slices
			float sliceAngle = Mathf.Asin(thickness/radius)*Mathf.Rad2Deg;

			// Find number of slices needed
			float numSlices = 360/sliceAngle;

			// Make copies of the original slice and position them radially
			for (int i = 1; i < numSlices; i++)
			{
				var sliceCopy = Object.Instantiate(bowlSlice);

				sliceCopy.transform.Rotate(0f, sliceAngle*i, 0f, Space.World);

				// Set the slice copy's parent to the compound collider game object
				sliceCopy.transform.SetParent(compoundCollider.transform);
			}

			// After copying original slice, set the original slice's parent to the compound collider game object
			bowlSlice.transform.SetParent(compoundCollider.transform);

			// Register undo event for creation of compound collider
			Undo.RegisterCreatedObjectUndo(bowlSlice, "Create Bowl Compound Collider");

			return compoundCollider;
		}

		/// <summary>
		/// Returns a gameobject with a cylindrical mesh and meshcollider with the given number of sides.
		/// Not to be confused with Unity-standard Cylinder primitive.
		/// </summary>
		static GameObject CreateCylinderPrimitive(int sides, float radius, float height)
		{
			var cylinder = GameObject.CreatePrimitive(PrimitiveType.Plane);

			// Generate the cylinder mesh, and assign it to the meshcollider
			var cylinderMesh = CreateCylinderMesh(sides, radius, height);
			var meshCollider = cylinder.GetComponent<MeshCollider>();
			meshCollider.sharedMesh = cylinderMesh;

			// Set mesh collider as convex so it can be used with rigidbody physics
			meshCollider.convex = true;

			// Set the cylinder mesh as the rendered mesh too
			var meshFilter = cylinder.GetComponent<MeshFilter>();
			meshFilter.mesh = cylinderMesh;

			return cylinder;
		}

		/// <summary>
		/// Returns a cylindrical mesh to fit a ring shape with given radius. Cylinder will have the given number of sides, and height.
		/// </summary>
		static Mesh CreateCylinderMesh(int sides, float radius, float height)
		{
			// circumradius = r sec(π/n)
			var circumradius = radius * 1 / Mathf.Cos(Mathf.PI / sides);

			var cylinderMesh = MeshE.Cylinder(circumradius, sides, height);

			cylinderMesh.Rotate(Quaternion.Euler(0f, (360f / sides) / 2f, 0f));

			return cylinderMesh;
		}
	}
}