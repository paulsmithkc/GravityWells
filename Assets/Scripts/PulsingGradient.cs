using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingGradient : MonoBehaviour {

    public float EdgeMultiplier = 10;
    public float EdgeMultiplierMin = 5;
    public float EdgeMultiplierMax = 10;
    public float EdgeMultiplierFreq = 1;
    private float Theta = 0;
    public Renderer renderer = null;

    // Use this for initialization
    void Start() {
        Theta = Random.Range(0, 1);
    }
	
	// Update is called once per frame
	void Update() {
        float deltaTime = Time.deltaTime;
        float tau = Mathf.PI * 2;

        Theta = (Theta + deltaTime * EdgeMultiplierFreq) % 1;
        EdgeMultiplier = Mathf.Lerp(
            EdgeMultiplierMin,
            EdgeMultiplierMax,
            Mathf.Sin(Theta * tau) * 0.5f + 1.0f
        );

        if (renderer != null)
        {
            renderer.material.SetFloat("_EdgeMultiplier", EdgeMultiplier);
        }
    }
}
