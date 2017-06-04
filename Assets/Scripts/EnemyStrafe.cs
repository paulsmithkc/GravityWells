using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrafe : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public PlanetGravitySource planetGravity = null;
    public GameObject player = null;
    public float moveSpeed = 0.0f;
    public float turnSpeed = 0.0f;
    public float smoothTime = 0.15f;

    private const float maxAccel = 1000.0f;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 currentMoveVelocity = Vector3.zero;
    private Vector3 currentMoveAccel = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    // Use this for initialization
    void Start() {
        //rigidbody = GetComponent<Rigidbody>();
        planetGravity = FindObjectOfType<PlanetGravitySource>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentMoveVelocity = Vector3.zero;
        currentMoveAccel = Vector3.zero;
        targetRotation = rigidbody.rotation;
        StartCoroutine(RandomizeDirection());
    }

    IEnumerator RandomizeDirection()
    {
        while (enabled)
        {
            float h = 0.5f * Random.Range(-2, 2);
            targetVelocity = new Vector3(h, 0, 0) * moveSpeed;
            //Vector2 v = Random.insideUnitCircle;
            //targetVelocity = new Vector3(v.x, 0, v.y) * moveSpeed;
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    // Update is called once per frame
    void Update() {
        float deltaTime = Time.deltaTime;
        currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, targetVelocity, ref currentMoveAccel, smoothTime, maxAccel, deltaTime);
    }

    void FixedUpdate()
    {
        Vector3 forward = (player.transform.position - rigidbody.position).normalized;
        Vector3 upwards = (rigidbody.position - planetGravity.transform.position).normalized;
        targetRotation = Quaternion.LookRotation(forward, upwards);
        //Quaternion.FromToRotation(transform.forward, forward) * rigidbody.rotation;

        float deltaTime = Time.fixedDeltaTime;
        rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, targetRotation, turnSpeed));
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(currentMoveVelocity) * deltaTime);
    }
}
