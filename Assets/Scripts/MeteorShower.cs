using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShower : MonoBehaviour {

    public Meteor meteorPrefab = null;
    public float meteorsPerSecond = 5;
    public float meteorSpawnRadius = 150;
    private float meteorsToSpawn = 0;
    private Stack<Meteor> inactiveMeteors = new Stack<Meteor>();

    // Use this for initialization
    void Start () {
        meteorsToSpawn = meteorsPerSecond;
    }
	
	// Update is called once per frame
	void Update () {
        float deltaTime = Time.deltaTime;
        meteorsToSpawn += deltaTime * meteorsPerSecond;

        while (meteorsToSpawn >= 1)
        {
            InstantiateMeteor(Random.onUnitSphere * meteorSpawnRadius);
            meteorsToSpawn -= 1.0f;
        }
    }

    public Meteor InstantiateMeteor(Vector3 pos)
    {
        Vector3 meteorForward = (transform.position - pos).normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, meteorForward);

        Meteor meteor;
        if (inactiveMeteors.Count > 0)
        {
            meteor = inactiveMeteors.Pop();
            meteor.transform.position = pos;
            meteor.transform.rotation = rotation;
        }
        else
        {
            meteor = GameObject.Instantiate(meteorPrefab, pos, rotation);
        }

        meteor.Reset(this);
        return meteor;
    }

    public void DestroyMeteor(Meteor meteor)
    {
        meteor.gameObject.SetActive(false);
        inactiveMeteors.Push(meteor);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, meteorSpawnRadius);
    }
}
