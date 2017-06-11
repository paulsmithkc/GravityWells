using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingGradient : MonoBehaviour {

    public float EdgeMultiplierMin = 5;
    public float EdgeMultiplierMax = 10;
    public float EdgeMultiplierFreq = 1;
    public Color MinColor = Color.white;
    public Color MaxColor = Color.white;
    private float Theta = 0;
    private float CurrentEdgeMultiplier = 10;
    private Color CurrentColor = Color.white;
    public Renderer Renderer = null;

    // Use this for initialization
    void Start() {
        Theta = Random.Range(0, 1);
    }
	
	// Update is called once per frame
	void Update() {
        float deltaTime = Time.deltaTime;
        float tau = Mathf.PI * 2;
        Theta = (Theta + deltaTime * EdgeMultiplierFreq) % 1;

        float t = Mathf.Sin(Theta * tau) * 0.5f + 1.0f;
        CurrentEdgeMultiplier = Mathf.Lerp(EdgeMultiplierMin, EdgeMultiplierMax, t);
        CurrentColor = Color.Lerp(MinColor, MaxColor, t);

        if (Renderer != null)
        {
            var material = Renderer.material;
            material.SetFloat("_EdgeMultiplier", CurrentEdgeMultiplier);
            material.SetColor("_Color", CurrentColor);
        }
    }
}
