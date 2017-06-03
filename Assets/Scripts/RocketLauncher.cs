using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour {
    
    public Rocket rocketPrefab;
    public string fireButton = "Fire1";
    
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(fireButton)) {
            GameObject.Instantiate(
                rocketPrefab,
                transform.position + transform.forward * rocketPrefab.initialSpeed * 0.15f,
                transform.rotation
            );
        }
	}
}
