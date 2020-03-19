//	=============================
//	 Compound Collider Generator 
//	=============================
//	Version 1.1
//  Copyright (c) 2016 Kyle Vekos
//	Unity Asset Store Page: http://u3d.as/tjD
//	=============================

using System;
using UnityEngine;
using UnityEditor;

namespace CompoundColliderGenerator
{
	public class CCGEditorWindow : EditorWindow
	{
		#region Enums

		public enum EditorStatus
		{
			Ready,
			Editing,
			NonUniformScale,
			InvalidSelection
		};

		#endregion

		// Editor fields
		private GameObject workingCollider = null;
		EditorStatus editorStatus = EditorStatus.InvalidSelection;
		CompoundShape compoundShape = CompoundShape.Ring;
		ColliderShape colliderShape = ColliderShape.Box;
		bool selectedHasMesh = false;
		bool meshBoundsAsCenter = true;
		bool renderersDisabled = false;

		// Shared shape fields
		int segments = 6;
		float outerRadius = 2f;
		float height = 1f;
		float wallThickness = 0.1f;

		// Ring shape only fields
		PivotAxis pivotAxis = PivotAxis.Y;
		float innerRadius = 1.5f;
		bool lockInnerRadiusToOuter = false;
		bool matchHeightToWidth = true;
		bool hasEndCap = false;
		float rotationOffset = 0f;

		// Box shape only fields
		float length = 1f;
		float width = 1f;

		// Bowl shape only fields
		float range = 180f;

		[MenuItem("Tools/Compound Collider Generator")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			CCGEditorWindow window =
				(CCGEditorWindow) EditorWindow.GetWindow(typeof(CCGEditorWindow));

			// Set the Window title
			window.titleContent.text = "Compund Collider Generator";

			// Set minimum window size
			window.minSize = new Vector2(275, 346);

			window.Show();
		}		

		void OnSelectionChange()
		{
			// Clear the working collider reference
			workingCollider = null;

			// Unsubscribe from undo events
			Undo.undoRedoPerformed -= OnUndoRedo;

			if (Selection.activeGameObject != null)
			{
				selectedHasMesh = Selection.activeGameObject.GetComponent<MeshFilter>() != null;
				meshBoundsAsCenter = selectedHasMesh;
			}

			// Re-draw the editor window to reflect changes in GUI
			Repaint();
		}

		void SetEditorStatus()
		{
			// If we already have a working collider then we're in the middle of Editing
			if (workingCollider != null)
			{
				this.editorStatus = EditorStatus.Editing;
			}
			else if (Selection.activeGameObject != null)
			{
				var t = Selection.activeGameObject.transform;

				// Check for non-uniform scale on target object and all its ancestors
				while (t != null)
				{
					var targetObjectScale = t.localScale;

					if (Math.Abs(targetObjectScale.x - targetObjectScale.y) > 0.0001f ||
						Math.Abs(targetObjectScale.x - targetObjectScale.z) > 0.0001f)
					{
						this.editorStatus = EditorStatus.NonUniformScale;
						return;
					}

					t = t.parent;
				}

				this.editorStatus = EditorStatus.Ready;
			}
			else
			{
				// This usually means that nothing is selected in the editor
				this.editorStatus = EditorStatus.InvalidSelection;
			}
		}

		/// <summary>
		/// Draws a box with text indicating the state of the tool.
		/// </summary>
		void DrawStatusText()
		{
			string statusText;
			Color statusColor;

			switch (editorStatus)
			{
				case EditorStatus.Editing:
					statusColor = new Color(1f, 0.5f, 0f); // Orange
					statusText = "Editing";
					break;
				case EditorStatus.Ready:
					statusColor = Color.green;
					statusText = "Ready";
					break;
				case EditorStatus.NonUniformScale:
					statusColor = Color.red;
					statusText = "Object or ancestor has non-uniform scale!";
					break;
				default:
					statusColor = Color.yellow;
					statusText = "Select an Object";
					break;
			}

			GUI.color = statusColor;
			GUILayout.Box(statusText, GUILayout.ExpandWidth(true));
			GUI.color = Color.white;
		}

		void DrawRingGUI()
		{
			// Store distance of inner from outer radius to lock distance if enabled
			var radiusDiff = outerRadius - innerRadius;

			GUILayout.Label("Ring", EditorStyles.boldLabel);
			pivotAxis = (PivotAxis)EditorGUILayout.EnumPopup("Pivot Axis", pivotAxis);
			segments = EditorGUILayout.IntSlider("Sides", segments, 3, 64);
			outerRadius = Mathf.Max(EditorGUILayout.FloatField("Outer Radius", outerRadius), 0.011f); // 0.011-Infinity

			EditorGUILayout.BeginHorizontal();
			if (lockInnerRadiusToOuter)
				GUI.enabled = false;
			innerRadius = Mathf.Clamp(EditorGUILayout.FloatField("Inner Radius", innerRadius), 0.01f, outerRadius - 0.01f); // 0.01-(outerRadius - 0.01)
			GUI.enabled = true;

			lockInnerRadiusToOuter =
				GUILayout.Toggle(lockInnerRadiusToOuter,
					new GUIContent("Lock", "Locks inner radius value to current distance from outer radius."),
					"Button",
					GUILayout.MaxWidth(50f));

			EditorGUILayout.EndHorizontal();

			// Lock innerRadius to current distance from outerRadius
			if (lockInnerRadiusToOuter)
			{
				innerRadius = Mathf.Max(0.1f, outerRadius - radiusDiff);
			}

			colliderShape =
				(ColliderShape)
					EditorGUILayout.EnumPopup(new GUIContent("Collider Shape", "Sets the shape of the sub-collider objects."),
						colliderShape);

			// Options for box collider only
			if (colliderShape == ColliderShape.Box)
			{
				EditorGUILayout.Separator();

				matchHeightToWidth =
					EditorGUILayout.Toggle(
						new GUIContent("Match Height to Width", "Lock height to Outer Radius minus Inner Radius."), matchHeightToWidth);
				if (matchHeightToWidth)
					GUI.enabled = false;
				height = Mathf.Max(EditorGUILayout.FloatField("Height", height), 0.01f); // 0.01-Infinity
				GUI.enabled = true;

				hasEndCap =
					EditorGUILayout.Toggle(
						new GUIContent("Cap Bottom", "Adds a cylindrical cap to the bottom end of the ring, creating a barrel shape."),
						hasEndCap);

				EditorGUILayout.Separator();

				rotationOffset =
					EditorGUILayout.Slider(
						new GUIContent("Rotation", "Apply rotation to sub-colliders around their length-wise axis."), rotationOffset, 0f, 360f);
			}
		}

		void DrawBoxGUI()
		{
			GUILayout.Label("Box", EditorStyles.boldLabel);

			length = Mathf.Max(EditorGUILayout.FloatField("Length", length), 0.01f); // 0.01-Infinity
			width = Mathf.Max(EditorGUILayout.FloatField("Width", width), 0.01f); // 0.01-Infinity
			height = Mathf.Max(EditorGUILayout.FloatField("Height", height), 0.01f); // 0.01-Infinity

			wallThickness = Mathf.Clamp(EditorGUILayout.FloatField("Wall Thickness", wallThickness), 0.01f,
				Mathf.Min(width, length) / 2);
		}

		void DrawBowlGUI()
		{
			GUILayout.Label("Bowl", EditorStyles.boldLabel);

			outerRadius = Mathf.Max(EditorGUILayout.FloatField("Radius", outerRadius), 0.011f); // 0.011-Infinity
			segments = EditorGUILayout.IntSlider("Sides", segments, 3, 64);
			wallThickness = Mathf.Clamp(EditorGUILayout.FloatField("Wall Thickness", wallThickness), 0.1f, outerRadius);
			range = EditorGUILayout.Slider("Range", range, 1f, 360f);
		}

		void DrawGUIControls()
		{
			EditorGUILayout.Separator();

			compoundShape = (CompoundShape)EditorGUILayout.EnumPopup("Shape to Generate", compoundShape);

			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

			if (compoundShape == CompoundShape.Ring)
			{
				DrawRingGUI();
			}
			else if (compoundShape == CompoundShape.Box)
			{
				DrawBoxGUI();
			}
			else if (compoundShape == CompoundShape.Bowl)
			{
				DrawBowlGUI();
			}

			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

			GUILayout.Label("Options", EditorStyles.boldLabel);

			// Only allow this option if the selected gameObject has a mesh
			if (!selectedHasMesh)
				GUI.enabled = false;
			meshBoundsAsCenter =
				EditorGUILayout.Toggle(
					new GUIContent("Mesh Bounds as Center", "Center compound collider at Mesh component bounds center."),
					meshBoundsAsCenter);
			GUI.enabled = true;

			if (colliderShape != ColliderShape.Capsule || compoundShape == CompoundShape.Box)
			{
				renderersDisabled = EditorGUILayout.Toggle("Disable Renderers", renderersDisabled);
			}
			else
			{
				// Capsule meshes don't scale like the colliders do and are misleading, so always disable them
				renderersDisabled = true;
			}

			// Update the editor status since the last few controls depend on this
			SetEditorStatus();
		}

		/// <summary>
		/// Draw the CCG editor window GUI.
		/// </summary>
		void OnGUI()
		{
			EditorGUI.BeginChangeCheck(); // <-- Start checking controls for changes to allow live updates
			DrawGUIControls();
			if (EditorGUI.EndChangeCheck() && editorStatus == EditorStatus.Editing) // <-- Control checking ends here
			{
				UpdateCompoundCollider();
			}

			DrawStatusText();

			if (editorStatus == EditorStatus.Editing)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Finish"))
				{
					// Clear the working collider reference, ending the editing state
					workingCollider = null;
				}
				if (GUILayout.Button("Cancel"))
				{
					// Delete the working collider, cancelling its creation
					GameObject.DestroyImmediate(workingCollider);

					// Clear the working collider reference, ending the editing state
					workingCollider = null;
				}
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				if (editorStatus != EditorStatus.Ready)
					GUI.enabled = false;

				if (GUILayout.Button("Create"))
				{
					CreateCompoundCollider();
				}

				GUI.enabled = true;
			}
		}

		private void CreateCompoundCollider()
		{
			var selectedGameObject = Selection.activeGameObject;

			// Abort create if the selected game object is null
			if (selectedGameObject == null)
				return;

			GameObject compoundCollider = null;

			if (compoundShape.Equals(CompoundShape.Ring))
			{
				// Match height to width if enabled
				float ringHeight = matchHeightToWidth ? (outerRadius - innerRadius) : height;

				compoundCollider =
					CompoundCollider.CreateRing(pivotAxis, segments, outerRadius, innerRadius, colliderShape, ringHeight, rotationOffset, hasEndCap);
			}
			else if (compoundShape.Equals(CompoundShape.Box))
			{
				compoundCollider = CompoundCollider.CreateBox(length, width, height, wallThickness);
			}
			else if (compoundShape.Equals(CompoundShape.Bowl))
			{
				compoundCollider = CompoundCollider.CreateBowl(outerRadius, segments, wallThickness, range);
			}

			// Move the collider so its position is relative to the selected game object
			compoundCollider.transform.position += selectedGameObject.transform.position;

			// If option is enabled, move collider so its position is relative to the select game object's mesh bounds center
			if (selectedHasMesh && meshBoundsAsCenter)
			{
				// NOTE: Mesh bounds center is local space
				compoundCollider.transform.position += selectedGameObject.GetComponent<MeshFilter>().sharedMesh.bounds.center;
			}

			// Temporarily set the selected game object's rotation to zero to avoid colliders being warped when collider's parent is set
			var originalRotation = selectedGameObject.transform.rotation;
			selectedGameObject.transform.rotation = Quaternion.identity;

			// Set the collider's parent to the selected game object
			compoundCollider.transform.SetParent(selectedGameObject.transform);

			// Restore the select game object's original rotation
			selectedGameObject.transform.rotation = originalRotation;

			if (renderersDisabled)
			{
				// Disable mesh renderers
				var meshRenderers = compoundCollider.GetComponentsInChildren<MeshRenderer>();
				foreach (var meshRenderer in meshRenderers)
				{
					meshRenderer.enabled = false;
				}
			}

			// Set created collider to working collider
			workingCollider = compoundCollider;

			// Subscribe to undo events so we know when to clear reference to working collider
			Undo.undoRedoPerformed += OnUndoRedo;
		}

		private void UpdateCompoundCollider()
		{
			// Delete the working collider if it exists
			if (workingCollider != null)
				GameObject.DestroyImmediate(workingCollider);

			// Create a new collider
			CreateCompoundCollider();
		}

		private void OnUndoRedo()
		{
			// Clear reference to working collider
			workingCollider = null;

			// Unsubscribe from future undo events
			Undo.undoRedoPerformed -= OnUndoRedo;
		}

		/*
		// "This function is called when the object is loaded."
		void OnEnable()
		{
		}
		*/
	}
}