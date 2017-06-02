using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{

    public float gravityScale;

    private void OnTriggerStay(Collider other)
    {
        // If the object has a rigidbody...
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb)
        {
            // Apply gravity!
            Vector3 dist = transform.position - other.transform.position;
            Vector3 force = dist.normalized * gravityScale;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }

}
