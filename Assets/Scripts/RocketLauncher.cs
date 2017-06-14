using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour {
    
    public Rocket rocketPrefab;
    public string fireButton = "Fire1";
    public float spreadFactor = 1;
    public bool allowPlayerControl = true;
    
	// Update is called once per frame
	void Update () {
		if (allowPlayerControl && Input.GetButtonDown(fireButton)) {
            var r = Random.insideUnitCircle * spreadFactor;
            GameObject.Instantiate(
                rocketPrefab,
                transform.position 
                  + transform.forward * rocketPrefab.initialSpeed * 0.15f 
                  + transform.right * r.x
                  + transform.up * r.y,
                transform.rotation
            );
        }
	}
}
