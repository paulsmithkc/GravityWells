using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;
    public float healthPerSecond;
    private float healthLastFrame;

    void Start ()
    {
        currentHealth = maxHealth;
        healthLastFrame = maxHealth;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        currentHealth = Mathf.Clamp(
            currentHealth + healthPerSecond * deltaTime,
            0, maxHealth
        );

        float dps = (healthLastFrame - currentHealth) / deltaTime;
        if (dps > 0 && !float.IsNaN(dps) && !float.IsInfinity(dps))
        {
            //Debug.Log(dps);
            Camera.main.SendMessage("Shake", dps);
        }
        healthLastFrame = currentHealth;
    }

    public void DamagePlayer(float damage)
    {
        currentHealth -= damage;
    }
}
