using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFlameTest : MonoBehaviour
{
    public GameObject reactiveMetalsObjects;
    public GameObject flameTestObjects;

    //Use for init
    void Start()
    {

    }

    // Called by GazeGestureManager when the user performs a select gesture
    void OnSelect()
    {
        reactiveMetalsObjects.SetActive(false);
        flameTestObjects.SetActive(true);

    }



}
