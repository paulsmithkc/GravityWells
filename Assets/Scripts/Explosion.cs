using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float explosionRadius = 10;
    public float explosionForce = 100;
    public float explosionDamage = 0;
    public float explosionDuration = 1;
    //public AudioClip explosionSound = null;
    //public float explosionVolume = 1;
    private PlayerMovement player;

    void Start()
    {
        float scale = explosionRadius;
        transform.localScale = new Vector3(scale, scale, scale);

        //if (explosionSound != null)
        //{
        //    AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);
        //}
        //else
        //{
        //    //Debug.Log("BOOM");
        //}

        player = GameObject.FindObjectOfType<PlayerMovement>();

        if (explosionDamage > 0)
        {
            Vector3 pos = transform.position;
            int planetMask = ~(1 << LayerMask.NameToLayer("Planet"));
            var objects = Physics.OverlapSphere(pos, explosionRadius, planetMask, QueryTriggerInteraction.Ignore);
            foreach (var o in objects)
            {
                var ph = o.GetComponent<PlayerHealth>();
                if (ph)
                {
                    ph.DamagePlayer(explosionDamage);
                }
                var eh = o.GetComponent<EnemyHealth>();
                if (eh)
                {
                    eh.DamageEnemy(explosionDamage);
                }
            }
        }
    }

    void Update() {
        float deltaTime = Time.deltaTime;
        explosionDuration -= deltaTime;

        if (player != null)
        {
            transform.rotation = Quaternion.LookRotation(-player.transform.forward, player.transform.up);
        }

        Vector3 pos = transform.position;
        int planetMask = ~(1 << LayerMask.NameToLayer("Planet"));
        var objects = Physics.OverlapSphere(pos, explosionRadius, planetMask, QueryTriggerInteraction.Ignore);
        foreach (var o in objects)
        {
            Rigidbody rb = o.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(explosionForce, pos, explosionRadius, 0, ForceMode.Force);
            }
        }

        if (explosionDuration <= 0) { GameObject.Destroy(this.gameObject); }
    }
}
