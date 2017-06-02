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
    }
	
	// Update is called once per frame
	void Update () {
        if (rigidbody == null) {
            transform.Rotate(axis, rotationSpeed * Time.deltaTime);
        } else {
            rigidbody.angularVelocity = new Vector3(
                0.0f, rotationSpeed * Mathf.Deg2Rad, 0.0f
            );
        }
	}
}
