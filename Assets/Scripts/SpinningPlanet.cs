using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlanet : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public float rotationSpeed = 0;
    public Vector3 axis = Vector3.up;
    
    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.MoveRotation(Quaternion.FromToRotation(
            transform.up, axis
        ));
    }
	
	void FixedUpdate() {
        if (rotationSpeed != 0.0f)
        {
            float deltaTime = Time.fixedDeltaTime;
            var a = axis.normalized;
            if (rigidbody == null)
            {
                transform.Rotate(a, rotationSpeed * deltaTime);
            }
            else {
                rigidbody.MoveRotation(
                    Quaternion.AngleAxis(rotationSpeed * deltaTime, a) * rigidbody.rotation
                );
            }
        }
	}

    void OnDrawGizmos() {
        var a = axis.normalized;
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, a * 110);
        Gizmos.DrawRay(transform.position, a * -110);
    }
}
