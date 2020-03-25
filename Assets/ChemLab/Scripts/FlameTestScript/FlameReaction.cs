using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameReaction : MonoBehaviour
{
    public GameObject flame;

    public GameObject FlameTestS3;

    void OnStart()
    {
        flame.GetComponent<ParticleSystem>().startColor = new Color(32f / 255f, 40f / 255f, 255f / 255f, 1f);

    }

    void OnTriggerEnter(Collider collider)
    {
        string objName = collider.gameObject.name;

        switch (objName)
        {
            case "KSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, .6517f);
                FlameTestS3.SetActive(true);
                break;
            case "BaSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(.05473477f, 0.3920533f, 0.7735849f);
                FlameTestS3.SetActive(true);
                break;
            case "CuSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(0.06666664f, 0.7960784f, 0.271321f);
                FlameTestS3.SetActive(true);
                break;
            case "CaSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(0.8679245f, 0.1211999f, 0.07778566f);
                FlameTestS3.SetActive(true);
                break;
            default:
                flame.GetComponent<ParticleSystem>().startColor = new Color(32f / 255f, 40f / 255f, 255f / 255f, 1f);
                FlameTestS3.SetActive(true);
                break;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        flame.GetComponent<ParticleSystem>().startColor = new Color(32f / 255f, 40f / 255f, 255f / 255f, 1f);

    }
}
