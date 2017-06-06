using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public new ParticleSystem particleSystem = null;
    public MeteorShower meteorShower = null;
    public float initialSpeed = 10;
    public float initialTimer = 10;
    public float timer = 10;
    public float cullingRadius = 60;
    public Explosion explosionPrefab = null;
    public bool explodeOnImpact = true;
    public float explosionRadius = 10;
    public float explosionForce = 100;
    private bool exploded = false;

    private CullingGroup cullingGroup;

    public void Start()
    {
        rigidbody.velocity = transform.forward * initialSpeed;
        timer = initialTimer;
        exploded = false;

        StartCoroutine("WaitForTimer");
        gameObject.SetActive(true);

        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = Camera.main;
        cullingGroup.SetBoundingSpheres(
            new BoundingSphere[] { new BoundingSphere(transform.position, cullingRadius) }
        );
        cullingGroup.SetBoundingSphereCount(1);
        cullingGroup.onStateChanged += OnCullingStateChanged;
    }

    private void OnCullingStateChanged(CullingGroupEvent e)
    {
        if (exploded) { return; }
        if (e.isVisible) {
            particleSystem.Play(true);
        } else {
            particleSystem.Pause();
        }
    }

    public IEnumerator WaitForTimer()
    {
        yield return new WaitForSeconds(timer);
        Explode();
        yield return null;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(transform.position, transform.forward * initialSpeed);
    //    Gizmos.DrawRay(transform.position, rigidbody.velocity);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, cullingRadius);
    //}

    void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact)
        {
            //timer = Mathf.Min(timer, 0.15f);
            Explode();
        }
        else
        {
            particleSystem.Play(true);
            particleSystem.Stop(true);
        }
    }

    public void Explode()
    {
        if (exploded) { return; }
        exploded = true;

        Explosion exp = GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation);
        exp.explosionRadius = this.explosionRadius;
        exp.explosionForce = this.explosionForce;

        StopAllCoroutines();
        particleSystem.Stop(true);
        GameObject.Destroy(this.gameObject);
        //if (meteorShower != null) {
        //    meteorShower.DestroyMeteor(this);
        //} else {
        //    GameObject.Destroy(this.gameObject);
        //}
    }

    public void OnDestroy()
    {
        if (cullingGroup != null)
        {
            cullingGroup.Dispose();
            cullingGroup = null;
        }
    }
}
