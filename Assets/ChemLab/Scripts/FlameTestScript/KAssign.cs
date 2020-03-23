using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KAssign : MonoBehaviour
{
    public GameObject BaStick;
    public GameObject KStick;
    public GameObject LiStick;
    public GameObject CuStick;

    private Vector3 dipStickOrigPos;
    void OnStart()
    {
        
    }


    void OnSelect()
    {
        BaStick.SetActive(false);
        KStick.SetActive(true);
        LiStick.SetActive(false);
        CuStick.SetActive(false);
    }

}
