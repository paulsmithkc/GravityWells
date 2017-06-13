using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    public PlanetGravitySource planetGravity = null;
    public GameObject player = null;
    public float moveSpeed = 0.0f;
    public float turnSpeed = 0.0f;
    public float smoothTime = 0.15f;
    public Mode mode = Mode.STATIONARY;

    private const float maxAccel = 1000.0f;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 currentMoveVelocity = Vector3.zero;
    private Vector3 currentMoveAccel = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    public enum Mode
    {
        STATIONARY, RANDOM_DIR, STRAFE_CIRCLE, STRAFE_FLEE
    }

    // Use this for initialization
    void Start() {
        //rigidbody = GetComponent<Rigidbody>();
        planetGravity = FindObjectOfType<PlanetGravitySource>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentMoveVelocity = Vector3.zero;
        currentMoveAccel = Vector3.zero;
        targetRotation = rigidbody.rotation;
        SetMode(mode);
    }

    void Awake()
    {
        SetMode(mode);
    }

    public void SetMode(Mode m)
    {
        mode = m;
        StopAllCoroutines();
        switch (mode)
        {
            case Mode.STATIONARY:
                // do nothing
                break;
            case Mode.RANDOM_DIR:
                StartCoroutine(RandomDirectionMode());
                break;
            case Mode.STRAFE_CIRCLE:
                StartCoroutine(StrafeCircleMode());
                break;
            case Mode.STRAFE_FLEE:
                StartCoroutine(StrafeFleeMode());
                break;
        }
    }

    private IEnumerator RandomDirectionMode()
    {
        while (enabled)
        {
            Vector2 v = Random.insideUnitCircle;
            targetVelocity = new Vector3(v.x, 0, v.y) * moveSpeed;
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    private IEnumerator StrafeCircleMode()
    {
        while (enabled)
        {
            float h = 0.5f * Random.Range(-2, 3);
            targetVelocity = new Vector3(h, 0, 0) * moveSpeed;
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    private IEnumerator StrafeFleeMode()
    {
        while (enabled)
        {
            float h = 0.5f * Random.Range(-2, 3);
            float v = 0.25f * Random.Range(-4, 0);
            targetVelocity = new Vector3(h, 0, v) * moveSpeed;
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    // Update is called once per frame
    void Update() {
        if (rigidbody.useGravity)
        {
            float deltaTime = Time.deltaTime;
            currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, targetVelocity, ref currentMoveAccel, smoothTime, maxAccel, deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (rigidbody.useGravity)
        {
            float deltaTime = Time.fixedDeltaTime;
            rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, targetRotation, turnSpeed));
            rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(currentMoveVelocity) * deltaTime);

            Vector3 forward = (player.transform.position - rigidbody.position).normalized;
            Vector3 upwards = (rigidbody.position - planetGravity.transform.position).normalized;
            targetRotation = Quaternion.LookRotation(forward, upwards);
        }
    }
}
