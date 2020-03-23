using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayReactiveMetals : MonoBehaviour
{
    public GameObject instrStep1;
    public GameObject instrStep2;
    public GameObject instrStep3;

    public GameObject RMGeneralComponents;
    public GameObject reactiveMetalsObjects;
    public GameObject flameTestObjects;
    public GameObject na;
    public GameObject water;
    Vector3 naOrigPos;
    public GameObject li;
    Vector3 liOrigPos;
    public GameObject k;
    Vector3 kOrigPos;
    public GameObject cs;
    Vector3 csOrigPos;

    public GameObject bowl;
    public GameObject water2;

    public GameObject CsFormula;
    public GameObject KFormula;
    public GameObject LiFormula;
    public GameObject NaFormula;

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

        CsFormula.SetActive(false);
        KFormula.SetActive(false);
        LiFormula.SetActive(false);
        NaFormula.SetActive(false);
        na.SetActive(false);
        li.SetActive(false);
        k.SetActive(false);
        cs.SetActive(false);
        instrStep1.SetActive(true);
        instrStep2.SetActive(false);
        instrStep3.SetActive(false);
        reactiveMetalsObjects.SetActive(true);
        RMGeneralComponents.SetActive(true);
        flameTestObjects.SetActive(false);
        bowl.SetActive(true);
        water2.GetComponent<Renderer>().material.SetColor("_horizonColor", new Color(0f, 32f / 255f, 49f / 255f, 0f));
        water.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(7f / 255f, 34f / 255f, 51f / 255f, 238f / 255f));
    }



}
