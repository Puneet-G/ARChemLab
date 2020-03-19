using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KReaction : MonoBehaviour
{
    bool shrink = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

        shrink = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = new Vector3(0.0006f, 0.0006f, 0.0006f);
        Vector3 target = new Vector3(0f, 0f, 0f);
        if (shrink)
        {
            if (this.transform.localScale.x <= 0)
                shrink = false;
            else
                this.transform.localScale -= vector;
        }
    }


}
