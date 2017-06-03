using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableGravityWell : MonoBehaviour {
    
    public float gravityScale;
    public bool ignorePlayer;
    
    private void OnTriggerStay(Collider other)
    {
        if (ignorePlayer && string.Equals(other.gameObject.tag, "Player"))
        {
            return;
        }
        
        // If the object has a rigidbody...
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb)
        {
            // Apply gravity!
            Vector3 vec = (other.transform.position - this.transform.position);
            Vector3 dir = vec.normalized;
            Vector3 force = dir * -gravityScale / vec.sqrMagnitude;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }
}
