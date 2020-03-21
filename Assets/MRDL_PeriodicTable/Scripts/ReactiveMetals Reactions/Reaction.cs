using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    bool react = false;

    Transform entryPoint;
    Vector3 radialDirection;
    Vector3 originalScale;
    Transform center;
    float angle = 0;
    float centreOffset = 0;
    float speed = (2 * Mathf.PI) / 1; //2*PI in degress is 360, so you get 2 seconds to complete a circle
    float radius = 0.05f;
    GameObject reactionClone;
    Material water;
    float timeCollision = 0;
    Color originalColor;
    List<Rigidbody> destroyedPieces;
    bool exploded = false;

    public GameObject reactionEffect;
    public float shrinkTime;
    public float revTime;
    public Color finalColor;
    public GameObject equation;
    public GameObject questions;
    // Start is called before the first frame update
    void Start()
    {
        speed = (2 * Mathf.PI) / revTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "Water")
        {
            //Debug.Log("Found Water");
            react = true;
            entryPoint = collision.transform;
            center = collision.collider.transform;
            originalScale = transform.localScale;
            radialDirection = center.position - entryPoint.position;
            reactionClone = Instantiate(reactionEffect, transform.position, transform.rotation, transform);
            water = collision.collider.gameObject.GetComponent<Renderer>().material;
            originalColor = water.GetColor("_ReflectionColor");
            timeCollision = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(0f, 0f, 0f);
        if (react)
        {
            if (this.name == "Caesium")
            {
                timeCollision += Time.deltaTime;
                if (timeCollision >= 1.10f && !exploded)
                {
                    Debug.Log("In Explosion block");
                    exploded = true;
                    StartCoroutine(Explode());

                }
                //if(timeCollision >= 8)
                //{
                //    Destroy(reactionClone);
                //    //Destroy(destroyedPieces[0].transform.parent.gameObject);
                //    react = false;
                //}

            }
            else
            {
                timeCollision += Time.deltaTime;
                angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
                centreOffset += ((2 * Mathf.PI) / (shrinkTime / 2)) * Time.deltaTime;
                target.x = (Mathf.Cos(angle) * radius) + (center.position.x + 0.05f * Mathf.Sin(centreOffset));
                target.z = (Mathf.Sin(angle) * radius) + (center.position.z + 0.05f * Mathf.Cos(centreOffset));
                target.y = entryPoint.position.y;
                this.transform.position = target;
                //radialDirection = (center.position - target).normalized;
                if (this.transform.localScale.x <= 0)
                {
                    react = false;
                    equation.SetActive(true);
                    questions.SetActive(true);
                    Destroy(reactionClone);
                }
                else
                {
                    this.transform.localScale = Vector3.Lerp(originalScale, new Vector3(0f, 0f, 0f), timeCollision / shrinkTime); //(Time.deltaTime / shrinkTime) * originalScale;
                    water.SetColor("_ReflectionColor", Color.Lerp(originalColor, finalColor, timeCollision / shrinkTime));
                }
            }
        }
    }

    IEnumerator Explode()
    {
        Debug.Log("Explode");
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, 0.39f);
        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if (dest != null)
            {
                dest.Destroy();
                break;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.39f);
        foreach (Collider near in colliders)
        {
            Rigidbody rb = near.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (rb.name == "Caesium") rb.isKinematic = true;
                else
                {
                    rb.AddExplosionForce(800, transform.position, 2.0f);
                    Debug.Log("Added Force");
                }
            }
        }
        yield return new WaitForSeconds(shrinkTime);
        Destroy(reactionClone);
        foreach (Collider piece in colliders)
        {
            if(piece.transform.parent != null && piece.transform.parent.gameObject.name == "bowl_broken_glass(Clone)")
            {
                Destroy(piece.transform.parent.gameObject);
                break;
            }
        }

        this.GetComponent<Rigidbody>().isKinematic = true;
        react = false;
        this.gameObject.SetActive(false);
        equation.SetActive(true);
        questions.SetActive(true);
    }


}
