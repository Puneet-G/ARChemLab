using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameReaction : MonoBehaviour
{
    public GameObject flame;
    

    void OnStart()
    {
        flame.GetComponent<ParticleSystem>().startColor = new Color(32f / 255f, 40f / 255f, 255f / 255f, 1f);

    }

    void OnTriggerEnter(Collider collider)
    {
        string objName = collider.gameObject.name;

        switch (objName)
        {
            case "LiSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(.75f, 0f, 0f);
                break;
            case "BaSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(0.06666664f, 0.7090117f, 0.7960784f);
                break;
            case "CuSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(0.06666664f, 0.7960784f, 0.271321f);
                break;
            case "KSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(224f / 255f, 33f / 255f, 224f / 255f);
                break;
            default:
                flame.GetComponent<ParticleSystem>().startColor = new Color(32f / 255f, 40f / 255f, 255f / 255f, 1f);
                break;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        flame.GetComponent<ParticleSystem>().startColor = new Color(32f / 255f, 40f / 255f, 255f / 255f, 1f);

    }
}
