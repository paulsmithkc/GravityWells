using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public float initialSpeed = 10;
    public float timer = 10;
    public float explosionRadius = 10;
    public float explosionForce = 100;
    private bool exploded = false;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * initialSpeed;
        exploded = false;
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
        if (!string.Equals(collision.gameObject.tag, "Planet"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (exploded) { return; }

        //Vector3 pos = transform.position;
        //int planetMask = ~(1 << LayerMask.NameToLayer("Planet"));
        //var objects = Physics.OverlapSphere(pos, explosionRadius, planetMask, QueryTriggerInteraction.Ignore);
        //foreach (var o in objects)
        //{
        //    Rigidbody rb = o.GetComponent<Rigidbody>();
        //    if (rb)
        //    {
        //        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0, ForceMode.Force);
        //    }
        //}
        
        GameObject.Destroy(this.gameObject);
        exploded = true;
    }
}
