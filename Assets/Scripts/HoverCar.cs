using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCar : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public PlanetGravitySource planetGravity = null;
    public float targetHeight = 4;
    public float hoverForce = 5;
    //public float currentHeight = 2;
    //public float currentVelocity = 0;

    //public Vector3 targetPos = Vector3.zero;
    //public Vector3 curentVelocity = Vector3.zero;
    //private Vector3 lastHitPoint = Vector3.zero;

    public Transform[] hoverPoints = new Transform[4];

    // Use this for initialization
    void Start () {
        planetGravity = FindObjectOfType<PlanetGravitySource>();
        //targetPos = transform.position;
        //curentVelocity = Vector3.zero;
    }

    //void Update()
    //{
    //    Vector3 currentPos = rigidbody.position;
    //    float maxDistance = 50.0f;
    //    int planetMask = 1 << LayerMask.NameToLayer("Planet");

    //    RaycastHit hitInfo;
    //    if (Physics.SphereCast(currentPos, 0.1f, -transform.up, out hitInfo, maxDistance, planetMask, QueryTriggerInteraction.Ignore))
    //    {
    //        lastHitPoint = hitInfo.point;
    //        targetPos = hitInfo.point + transform.up * targetHeight;
    //    } else {
    //        targetPos = currentPos;
    //    }

    //    //currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref curentVerticalSpeed, 0.15f, 20.0f, deltaTime);

    //    //float newHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref curentVerticalSpeed, 0.15f, 20.0f, deltaTime);
    //    //float deltaHeight = newHeight - currentHeight;
    //    //rigidbody.MovePosition(rigidbody.position + transform.up * deltaHeight);
    //    //currentHeight = newHeight;
    //}

    //void FixedUpdate()
    //{
    //    Vector3 planetUp;
    //    if (planetGravity != null)
    //    {
    //        planetUp = (this.transform.position - planetGravity.transform.position).normalized;
    //        rigidbody.rotation = Quaternion.FromToRotation(transform.up, planetUp) * rigidbody.rotation;
    //    }
    //    else
    //    {
    //        planetUp = transform.up;
    //    }

    //    //curentVelocity -= 9.8f * planetUp;

    //    float deltaTime = Time.fixedDeltaTime;
    //    Vector3 currentPos = rigidbody.position;
    //    currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref curentVelocity, 0.15f, 100.0f, deltaTime);
    //    rigidbody.MovePosition(currentPos);
    //}

    void FixedUpdate()
    {
        Vector3 planetUp;
        if (planetGravity != null)
        {
            planetUp = (this.transform.position - planetGravity.transform.position).normalized;
            rigidbody.MoveRotation(Quaternion.FromToRotation(transform.up, planetUp) * rigidbody.rotation);
        }
        else
        {
            planetUp = transform.up;
        }

        float deltaTime = Time.fixedDeltaTime;
        Vector3 currentPos = rigidbody.position;
        float maxDistance = targetHeight; //50.0f;
        int layerMask = 1 << LayerMask.NameToLayer("Planet");  //~(1 << LayerMask.NameToLayer("Enemy"));
        
        RaycastHit hitInfo;
        foreach (var t in hoverPoints)
        {
            if (Physics.Raycast(t.position, -planetUp, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                float force = hoverForce * (1.0f - (hitInfo.distance / targetHeight));
                rigidbody.AddForceAtPosition(planetUp * force, t.position, ForceMode.Acceleration);
            }
            //else if (currentPos.y > t.position.y)
            //{
            //    rigidbody.AddForceAtPosition(t.up * hoverForce, t.position, ForceMode.Acceleration);
            //}
            //else
            //{
            //    rigidbody.AddForceAtPosition(t.up * -hoverForce, t.position, ForceMode.Acceleration);
            //}
        }
    }

    void OnDrawGizmos()
    {
        foreach (var t in hoverPoints)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(t.position, 0.4f);
        }
    }
}
