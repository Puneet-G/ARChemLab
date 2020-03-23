using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsInteraction : MonoBehaviour
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

    public GameObject CsFormula;
    public GameObject KFormula;
    public GameObject LiFormula;
    public GameObject NaFormula;


    public GameObject instructionsStep2;
    public GameObject instructionsStep3;
    public GameObject water;
    public GameObject water2;
    public GameObject bowl;

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

        CsFormula.SetActive(false);
        KFormula.SetActive(false);
        LiFormula.SetActive(false);
        NaFormula.SetActive(false);

        instructionsStep2.SetActive(true);
        instructionsStep3.SetActive(false);
        na.SetActive(false);
        li.SetActive(false);
        k.SetActive(false);
        cs.SetActive(true);
        bowl.SetActive(true);
        water2.GetComponent<Renderer>().material.SetColor("_horizonColor", new Color(0f, 32f / 255f, 49f / 255f, 0f));
        water.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(7f / 255f, 34f / 255f, 51f / 255f, 238f / 255f));
    }
}