using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFlameTest : MonoBehaviour
{
    public GameObject reactiveMetalsObjects;
    public GameObject flameTestObjects;
    public GameObject[] sticks;

    public GameObject InfoPanel;

    //Use for init
    void Start()
    {
        foreach (GameObject s in sticks)
        {
            s.SetActive(false);
        }
    }

    // Called by GazeGestureManager when the user performs a select gesture
    void OnSelect()
    {
        reactiveMetalsObjects.SetActive(false);
        flameTestObjects.SetActive(true);

    }



}
