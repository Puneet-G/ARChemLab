using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaReaction : MonoBehaviour
{
    bool shrink = false;
    bool react = false;

    Transform entryPoint;
    Vector3 radialDirection;
    Transform center;
    float angle = 0;
    float centreOffset = 0;
    float speed = (2 * Mathf.PI) / 1; //2*PI in degress is 360, so you get 2 seconds to complete a circle
    float radius = 0.05f;

    public GameObject reactionEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "Water")
        {
            Debug.Log("Found Water");
            react = true;
            entryPoint = collision.transform;
            center = collision.collider.transform;
            radialDirection = center.position - entryPoint.position;
            Instantiate(reactionEffect, transform.position, transform.rotation, transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = new Vector3(0.00006f, 0.00006f, 0.00006f);
        Vector3 target = new Vector3(0f, 0f, 0f);
        if (react)
        {
            angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
            centreOffset += ((2 * Mathf.PI) / 4) * Time.deltaTime;
            target.x = (Mathf.Cos(angle) * radius) + (center.position.x + 0.05f * Mathf.Sin(centreOffset));
            target.z = (Mathf.Sin(angle) * radius) + (center.position.z + 0.05f * Mathf.Cos(centreOffset));
            Debug.Log("X offset:" + (0.001f * Mathf.Sin(centreOffset)).ToString());
            Debug.Log("Y offset:" + (0.001f * Mathf.Cos(centreOffset)).ToString());
            target.y = entryPoint.position.y;
            this.transform.position = target;
            radialDirection = (center.position - target).normalized;
            if (this.transform.localScale.x <= 0)
                react = false;
            else
                this.transform.localScale -= vector;
        }
    }


}
