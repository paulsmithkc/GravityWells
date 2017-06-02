using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlanet : MonoBehaviour {

    public float rotationSpeed = 0;
    public Vector3 axis = Vector3.up;
    public Rigidbody rigidbody = null;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.MoveRotation(Quaternion.FromToRotation(
            transform.up, axis
        ));
    }
	
	// Update is called once per frame
	void Update () {
        var a = axis.normalized;
        if (rigidbody == null) {
            transform.Rotate(a, rotationSpeed * Time.deltaTime);
        } else {
            rigidbody.MoveRotation(
                rigidbody.rotation * Quaternion.AngleAxis(
                    rotationSpeed * Time.deltaTime, a
                )
            );
        }
	}

    void OnDrawGizmos() {
        var a = axis.normalized;
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, a * 110);
        Gizmos.DrawRay(transform.position, a * -110);
    }
}
