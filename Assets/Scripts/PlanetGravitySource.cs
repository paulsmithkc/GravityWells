using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravitySource : MonoBehaviour
{
    public float gravityScale;
    public float gravityRadius;

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        int planetMask = ~(1 << LayerMask.NameToLayer("Planet"));
        var colliders = Physics.OverlapSphere(pos, gravityRadius, planetMask, QueryTriggerInteraction.Ignore);
        foreach (var c in colliders)
        {
            var rb = c.GetComponent<Rigidbody>();
            if (rb != null && rb.useGravity)
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

}
