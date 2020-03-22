using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    //Represents the hologram that is currently being gazed at
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    //Use for init
    void Awake()
    {
        Instance = this;

        //Set up a GestureRecognizer to detect Select gestures
        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            //Send onSelect message to focused object and its ancestors
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
            }
        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {
        //Figure out which hologram is focused in this frame
        GameObject oldFocusObject = FocusedObject;

        //Do a raycast into the world based on user's head position and orientation
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            //If raycast hit hologram use that as the focused object
            FocusedObject = hitInfo.collider.gameObject;

        }
        else
        {
            //If raycase did not hit hologram, clear focused object
            FocusedObject = null;
        }

        // If focused object changed this frame, start detecting fresh gestures again
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }

    }
}
