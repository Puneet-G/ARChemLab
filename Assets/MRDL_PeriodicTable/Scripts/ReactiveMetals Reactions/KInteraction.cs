using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KInteraction : MonoBehaviour
{
    public GameObject na;
    Vector3 naOrigPos;
    Quaternion naOrigRot;
    Vector3 naOrigScale;

    public GameObject li;
    Vector3 liOrigPos;
    Quaternion liOrigRot;
    Vector3 liOrigScale;

    public GameObject k;
    Vector3 kOrigPos;
    Quaternion kOrigRot;
    Vector3 kOrigScale;

    public GameObject cs;
    Vector3 csOrigPos;
    Quaternion csOrigRot;
    Vector3 csOrigScale;

    //Use for init
    void Start()
    {
        //Grab original local position of sphere when app starts
        naOrigPos = na.transform.localPosition;
        liOrigPos = li.transform.localPosition;
        kOrigPos = k.transform.localPosition;
        csOrigPos = cs.transform.localPosition;

        naOrigRot = na.transform.localRotation;
        liOrigRot = li.transform.localRotation;
        kOrigRot = k.transform.localRotation;
        csOrigRot = cs.transform.localRotation;

        naOrigScale = na.transform.localScale;
        liOrigScale = li.transform.localScale;
        kOrigScale = k.transform.localScale;
        csOrigScale = cs.transform.localScale;
    }

    // Called by GazeGestureManager when the user performs a select gesture
    void OnSelect()
    {
        na.transform.localPosition = naOrigPos;
        li.transform.localPosition = liOrigPos;
        k.transform.localPosition = kOrigPos;
        cs.transform.localPosition = csOrigPos;

        na.transform.localRotation = naOrigRot;
        li.transform.localRotation = liOrigRot;
        k.transform.localRotation = kOrigRot;
        cs.transform.localRotation = csOrigRot;

        na.transform.localScale = naOrigScale;
        li.transform.localScale = liOrigScale;
        k.transform.localScale = kOrigScale;
        cs.transform.localScale = csOrigScale;

        na.SetActive(false);
        li.SetActive(false);
        k.SetActive(true);
        cs.SetActive(false);
    }
}
