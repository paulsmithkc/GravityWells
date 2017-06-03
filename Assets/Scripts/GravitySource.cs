using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public float gravityScale;
    public bool affectRotation = false;

    private void OnTriggerStay(Collider other)
    {
        // If the object has a rigidbody...
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb)
        {
            // Apply gravity!
            Vector3 dir = (other.transform.position - this.transform.position).normalized;
            Vector3 force = dir * -gravityScale;
            rb.rotation = Quaternion.FromToRotation(other.transform.up, dir) * rb.rotation;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }

}
