using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;
    public float healthPerSecond;
    private float healthLastFrame;
    public GameObject debrisPrefab = null;
    public GameObject explosionPrefab = null;
    public bool exploded = false;

    void Start ()
    {
        currentHealth = maxHealth;
        healthLastFrame = maxHealth;
        exploded = false;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        if (!exploded)
        {
            currentHealth = Mathf.Clamp(
                currentHealth + healthPerSecond * deltaTime,
                0, maxHealth
            );
        }

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
        if (!exploded && currentHealth <= 0)
        {
            exploded = true;
            if (debrisPrefab != null)
            {
                GameObject.Instantiate(debrisPrefab, transform.position + transform.up, transform.rotation);
            }
            if (explosionPrefab != null)
            {
                var exp = GameObject.Instantiate(explosionPrefab, transform.position - 2 * transform.forward, transform.rotation);
                var ani = exp.GetComponent<Animator>();
                ani.speed = 0.25f;
            }

            var mgr = GameObject.FindObjectOfType<GameManager>();
            mgr.OnPlayerDeath(this);
        }
    }
}
