using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTube : MonoBehaviour
{
    Vector3 originalPosition;

    //Use for init
    void Start()
    {
        //Grab original local position of sphere when app starts
        originalPosition = this.transform.localPosition;
    }

    // Called by GazeGestureManager when the user performs a select gesture
    void OnSelect()
    {
        //If sphere has no RigidBody component, add one to enable physics
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    //Called by SpeechManager when the useer says the "Reset world" command
    void OnReset()
    {
        //If the sphere has a Rigidbody component, remove it to disable physics
        var rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
            Destroy(rigidbody);
        }

        //Put the sphere back into its original local position
        this.transform.localPosition = originalPosition;
    }

    //Called by SpeechManager when the user says the "Drop sphere" command
    void OnDrop()
    {
        OnSelect();
    }



}
