using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;
    //public float healthPerSecond;
    public Explosion explosionPrefab = null;
    private bool exploded = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    //void Update()
    //{
    //    float deltaTime = Time.deltaTime;
    //    currentHealth = Mathf.Clamp(
    //        currentHealth + healthPerSecond * deltaTime,
    //        0, maxHealth
    //    );
    //}

    public void DamageEnemy(float damage)
    {
        currentHealth -= damage;
        if (!exploded && currentHealth <= 0)
        {
            exploded = true;
            Debug.Log("BOOM");
            if (explosionPrefab != null)
            {
                GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation);
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}
