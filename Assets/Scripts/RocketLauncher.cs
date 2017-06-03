using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour {
    
    public Rocket rocketPrefab;
    public string fireButton = "Fire1";

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(fireButton)) {
            var r = GameObject.Instantiate(
                rocketPrefab,
                transform.position + transform.forward * rocketPrefab.initialSpeed * 0.15f,
                transform.rotation
            );
        }
	}
}
