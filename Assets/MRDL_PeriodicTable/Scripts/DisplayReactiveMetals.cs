using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayReactiveMetals : MonoBehaviour
{
    public GameObject RMGeneralComponents;
    public GameObject reactiveMetalsObjects;
    public GameObject flameTestObjects;
    public GameObject na;
    Vector3 naOrigPos;
    public GameObject li;
    Vector3 liOrigPos;
    public GameObject k;
    Vector3 kOrigPos;
    public GameObject cs;
    Vector3 csOrigPos;

    //Use for init
    void Start()
    {
        //Grab original local position of sphere when app starts
        naOrigPos = na.transform.localPosition;
        liOrigPos = li.transform.localPosition;
        kOrigPos = k.transform.localPosition;
        csOrigPos = cs.transform.localPosition;
    }

    // Called by GazeGestureManager when the user performs a select gesture
    void OnSelect()
    {
        na.transform.localPosition = naOrigPos;
        li.transform.localPosition = liOrigPos;
        k.transform.localPosition = kOrigPos;
        cs.transform.localPosition = csOrigPos;
        na.SetActive(false);
        li.SetActive(false);
        k.SetActive(false);
        cs.SetActive(false);
        reactiveMetalsObjects.SetActive(true);
        RMGeneralComponents.SetActive(true);
        flameTestObjects.SetActive(false);

    }



}
