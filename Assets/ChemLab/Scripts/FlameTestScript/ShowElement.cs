using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowElement : MonoBehaviour
{

    public string element;
    public GameObject BaStick;
    public GameObject CaStick;
    public GameObject KStick;
    public GameObject CuStick;

    
    public GameObject FlameTestS2;
    public GameObject FlameTestS3;



    void OnSelect()
    {
        switch (element)
        {
            case "Cu":
                BaStick.SetActive(false);
                CaStick.SetActive(false);
                KStick.SetActive(false);
                CuStick.SetActive(true);
                FlameTestS2.SetActive(true);
                FlameTestS3.SetActive(false);
                break;
            case "Ba":
                BaStick.SetActive(true);
                CaStick.SetActive(false);
                KStick.SetActive(false);
                CuStick.SetActive(false);
                FlameTestS2.SetActive(true);
                FlameTestS3.SetActive(false);
                break;
            case "K":
                BaStick.SetActive(false);
                CaStick.SetActive(false);
                KStick.SetActive(true);
                CuStick.SetActive(false);
                FlameTestS2.SetActive(true);
                FlameTestS3.SetActive(false);
                break;
            case "Ca":
                BaStick.SetActive(false);
                CaStick.SetActive(true);
                KStick.SetActive(false);
                CuStick.SetActive(false);
                FlameTestS2.SetActive(true);
                FlameTestS3.SetActive(false); 
                break;

        }
    }

    
}
