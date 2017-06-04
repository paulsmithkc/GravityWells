using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravitySource : MonoBehaviour
{
    public float gravityScale;
    public float gravityRadius;

    public void Start()
    {
    }

    public void OnDestroy()
    {
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        int planetMask = ~(1 << LayerMask.NameToLayer("Planet"));
        var colliders = Physics.OverlapSphere(pos, gravityRadius, planetMask, QueryTriggerInteraction.Ignore);
        foreach (var c in colliders)
        {
            var rb = c.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 vec = (rb.position - pos);
                Vector3 dir = vec.normalized;
                Vector3 force = dir * -gravityScale;
                rb.AddForce(force, ForceMode.Acceleration);
            }
            //if (string.Equals(c.gameObject.tag, "Player"))
            //{
            //    var player = c.GetComponent<PlayerMovement>();
            //    if (player != null)
            //    {
            //        player.planetGravity = this;
            //    }
            //}
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    // If the object has a rigidbody...
    //    Rigidbody rb = other.GetComponent<Rigidbody>();
    //    if (rb)
    //    {
    //        // Apply gravity!
    //        Vector3 vec = (other.transform.position - this.transform.position);
    //        Vector3 dir = vec.normalized;
    //        Vector3 force = dir * -gravityScale;
    //        //rb.rotation = Quaternion.FromToRotation(other.transform.up, dir) * rb.rotation;
    //        rb.AddForce(force, ForceMode.Acceleration);
    //    }

    //    if (string.Equals(this.gameObject.tag, "Planet") &&
    //        string.Equals(other.gameObject.tag, "Player"))
    //    {
    //        var player = other.GetComponent<PlayerMovement>();
    //        if (player != null)
    //        {
    //            player.planetGravity = this;
    //        }
    //    }
    //}

}
