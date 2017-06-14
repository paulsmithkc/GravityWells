using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;
    public float healthPerSecond;

    void Start ()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        currentHealth = Mathf.Clamp(
            currentHealth + healthPerSecond * deltaTime,
            0, maxHealth
        );
    }

    public void DamagePlayer(float damage)
    {
        currentHealth -= damage;
        Camera.main.SendMessage("Shake");
    }
}
