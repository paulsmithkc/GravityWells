using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlanet : MonoBehaviour {

    public float rotationSpeed = 0;
    public Vector3 axis = Vector3.up;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(axis, rotationSpeed * Time.deltaTime);
	}
}
