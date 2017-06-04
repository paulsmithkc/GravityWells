using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableGravityWell : MonoBehaviour {
    
    public float gravityScale;
    public float gravityRadius;
    public bool ignorePlayer;

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        int planetMask = ~(1 << LayerMask.NameToLayer("Planet"));
        var colliders = Physics.OverlapSphere(pos, gravityRadius, planetMask, QueryTriggerInteraction.Ignore);
        foreach (var c in colliders)
        {
            if (ignorePlayer && string.Equals(c.gameObject.tag, "Player"))
            {
                continue;
            }

            var rb = c.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 vec = (rb.position - pos);
                Vector3 dir = vec.normalized;
                float force = -gravityScale / vec.sqrMagnitude;
                if (!float.IsNaN(force) && !float.IsInfinity(force) && force != 0.0f)
                {
                    rb.AddForce(dir * force, ForceMode.Acceleration);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }
}
