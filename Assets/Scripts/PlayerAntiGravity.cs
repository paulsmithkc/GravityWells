using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAntiGravity : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public float antigravityMaxRadius = 20.0f;
    public float antigravityCurrentRadius = 2.0f;
    public Vector3 antigravityCurrentVelocity = Vector3.zero;
    public Vector3 antigravityCurrentAccel = Vector3.zero;
    private Vector3 lastHitPoint = Vector3.zero;

    void Update()
    {
        Vector3 targetVelocity = Vector3.zero;
        
        float cameraPitch = Input.GetAxis("Camera Pitch");
        antigravityCurrentRadius = Mathf.Clamp(antigravityCurrentRadius + cameraPitch, 0, antigravityMaxRadius);
        
        Vector3 pos = transform.position;
        int planetMask = 1 << LayerMask.NameToLayer("Planet");
        var planets = Physics.OverlapSphere(pos, antigravityCurrentRadius, planetMask, QueryTriggerInteraction.Ignore);
        foreach (var p in planets)
        {
            Vector3 dir = (p.transform.position - pos).normalized;
            RaycastHit hitInfo;
            if (Physics.SphereCast(pos, 0.1f, dir, out hitInfo, antigravityCurrentRadius, planetMask, QueryTriggerInteraction.Ignore))
            {
                lastHitPoint = hitInfo.point;
                rigidbody.MovePosition(hitInfo.point + dir * -antigravityCurrentRadius);
                //Vector3 d = (hitInfo.point + dir * -antigravityCurrentRadius - pos);
                //targetVelocity += 9.8f * d;
            }
        }
        
        antigravityCurrentVelocity = Vector3.SmoothDamp(antigravityCurrentVelocity, targetVelocity, ref antigravityCurrentAccel, 0.15f);
    }

    void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + antigravityCurrentVelocity * deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, antigravityCurrentRadius);
        Gizmos.DrawWireSphere(lastHitPoint, 0.1f);
    }
}
