using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameReaction : MonoBehaviour
{
    public GameObject flame;
    

    void OnStart()
    {
        flame.GetComponent<ParticleSystem>().startColor = new Color(184f/255f, 159f/255f, 81f/255f,1f);

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
                flame.GetComponent<ParticleSystem>().startColor = new Color(.553f, .714f, 0f);
                break;
            case "CuSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(.067f, .392f, .796f);
                break;
            case "KSalt":
                flame.GetComponent<ParticleSystem>().startColor = new Color(200f / 255f, 162f / 255f, 200f / 255f);
                break;
            default:
                flame.GetComponent<ParticleSystem>().startColor = new Color(184f / 255f, 159f / 255f, 81f / 255f, 1f);
                break;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        flame.GetComponent<ParticleSystem>().startColor = new Color(184f / 255f, 159f / 255f, 81f / 255f, 1f);

    }
}
