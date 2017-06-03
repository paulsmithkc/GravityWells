using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public MeteorShower meteorShower = null;
    public float initialSpeed = 10;
    public float initialTimer = 10;
    public float timer = 10;
    public float explosionRadius = 10;
    public float explosionForce = 100;
    private bool exploded = false;

    public void Reset(MeteorShower meteorShower)
    {
        this.meteorShower = meteorShower;
        rigidbody.velocity = transform.forward * initialSpeed;
        timer = initialTimer;
        exploded = false;
        this.transform.parent = meteorShower.transform;
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded) { return; }

        float deltaTime = Time.deltaTime;
        timer -= deltaTime;
        if (timer <= 0)
        {
            Explode();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * initialSpeed);
        Gizmos.DrawRay(transform.position, rigidbody.velocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        timer = Mathf.Min(timer, 5.0f);
    }

    public void Explode()
    {
        if (exploded) { return; }

        if (meteorShower != null) {
            meteorShower.DestroyMeteor(this);
        } else {
            GameObject.Destroy(this.gameObject);
        }
        exploded = true;
    }
}
