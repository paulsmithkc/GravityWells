using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDebrisSpawn : MonoBehaviour {

    public GameObject debrisPrefab = null;
    public float debrisSpawnRadius = 150;
    public float debrisToSpawn = 5;

    // Use this for initialization
    void Start () {
        while (debrisToSpawn >= 1)
        {
            InstantiateDebris(Random.onUnitSphere * debrisSpawnRadius);
            debrisToSpawn -= 1.0f;
        }
    }

    public GameObject InstantiateDebris(Vector3 pos)
    {
        Vector3 forward = (transform.position - pos).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, forward);

        GameObject debris = GameObject.Instantiate(debrisPrefab, pos, rotation);
        debris.transform.parent = this.transform;
        return debris;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, debrisSpawnRadius);
    }
}
