using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public float initialSpeed = 10;
    public float timer = 10;
    public Explosion explosionPrefab = null;
    public bool explodeOnImpact = true;
    public float explosionRadius = 10;
    public float explosionForce = 100;
    public float explosionDamage = 6;
    private bool exploded = false;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * initialSpeed;
        exploded = false;
        StartCoroutine("WaitForTimer");
    }
	
    public IEnumerator WaitForTimer()
    {
        yield return new WaitForSeconds(timer);
        Explode();
        yield return null;
    }

    void FixedUpdate()
    {
        rigidbody.MoveRotation(Quaternion.FromToRotation(transform.forward, rigidbody.velocity.normalized) * rigidbody.rotation);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * initialSpeed);

        if (rigidbody != null)
        {
            Gizmos.DrawRay(transform.position, rigidbody.velocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact && !string.Equals(collision.gameObject.tag, "Planet"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (exploded) { return; }
        exploded = true;

        if (explosionPrefab != null)
        {
            Explosion exp = GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation);
            exp.explosionRadius = this.explosionRadius;
            exp.explosionForce = this.explosionForce;
            exp.explosionDamage = this.explosionDamage;
        }

        StopAllCoroutines();
        GameObject.Destroy(this.gameObject);
    }
}
