using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{

    public float gravityScale;
    public float gravityRadius;

    private float radius;
    private float sqRadius;

	// Use this for initialization
	void Start ()
    {
        UpdateSqRadius();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // Get a list of game objects within gravity radius which have rigidbodies
        Attractable[] list = FindObjectsOfType<Attractable>();
        for (var i = 0; i < list.Length; i++)
        {
            // If the object has a rigidbody...
            Rigidbody rb = list[i].GetComponent<Rigidbody>();
            if (rb)
            {
                // And is in range...
                Vector3 dist = transform.position - rb.transform.position;
                if (dist.sqrMagnitude <= sqRadius)
                {
                    // Apply gravity!
                    Vector3 force = dist.normalized * gravityScale * list[i].gravityScale;
                    rb.AddForce(force, ForceMode.Acceleration);
                }
            }
        }
	}

    void UpdateSqRadius()
    {
        if (radius != gravityRadius)
        {
            radius = gravityRadius;
            sqRadius = radius * radius;
        }
    }
}
