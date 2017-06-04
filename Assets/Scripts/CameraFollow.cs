using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform cameraTarget;
    public float cameraSmoothTime = 0.15f;
    public Vector3 currentVelocity = Vector3.zero;
    private const float maxAccel = 1000.0f;

    // Use this for initialization
    void Start () {
        currentVelocity = Vector3.zero;
        transform.position = cameraTarget.position;
    }
	
    void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        Vector3 pos = transform.position;
        pos = Vector3.SmoothDamp(pos, cameraTarget.position, ref currentVelocity, cameraSmoothTime, maxAccel, deltaTime);
        transform.position = pos;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            cameraTarget.rotation,
            90.0f * deltaTime
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.DrawLine(transform.position, cameraTarget.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
