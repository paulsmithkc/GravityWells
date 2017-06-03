using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public MeteorShower meteorShower = null;
    public float initialSpeed = 10;
    public float initialTimer = 10;
    public float timer = 10;
    public bool explodeOnImpact = true;
    public float explosionRadius = 10;
    public float explosionForce = 100;
    private bool exploded = false;

    public void Reset(MeteorShower s)
    {
        meteorShower = s;
        rigidbody.velocity = transform.forward * initialSpeed;
        timer = initialTimer;
        exploded = false;

        transform.parent = meteorShower.transform;
        StartCoroutine("WaitForTimer");
        gameObject.SetActive(true);
    }

    public IEnumerator WaitForTimer()
    {
        yield return new WaitForSeconds(timer);
        Explode();
        yield return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * initialSpeed);
        Gizmos.DrawRay(transform.position, rigidbody.velocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact)
        {
            timer = Mathf.Min(timer, 1.0f);
        }
    }

    public void Explode()
    {
        if (exploded) { return; }

        StopAllCoroutines();
        GameObject.Destroy(this.gameObject);
        //if (meteorShower != null) {
        //    meteorShower.DestroyMeteor(this);
        //} else {
        //    GameObject.Destroy(this.gameObject);
        //}
        exploded = true;
    }
}
