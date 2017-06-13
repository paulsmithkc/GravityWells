using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float range = 10.0f;

    public float blinkRate = 0.3f;
    public float tellLength = 2.0f;
    public float attackLength = 1.0f;
    public float damagePerSecond = 0.5f;
    private bool telling = false;
    private bool attacking = false;

    // Attack cooldown
    public float cooldown = 5.0f;
    private float timeleft = 0.0f;

    public float tellWidth = 1.0f;
    public float attackWidth = 3.0f;

    private GameObject player;
    private GameManager mgr;
    private LineRenderer lr;
    private Vector3 target;

    public enum Phase { IDLE, TELLING, ATTACKING }
    private Phase _phase = Phase.IDLE;
    public Phase phase
    {
        get
        {
            return _phase;
        }
    }

    private EnemyMovement movement;
    private HoverCar hover;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        mgr = GameObject.FindObjectOfType<GameManager>();
        lr = GetComponentInChildren<LineRenderer>();

        hover = GetComponent<HoverCar>();
        movement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 playerPosition = mgr.playerPosition;

        float dist = Vector3.Distance(transform.position, playerPosition);
        if (dist > range)
        {
            if (_phase != Phase.IDLE) timeleft = cooldown;
            lr.enabled = false;
            _phase = Phase.IDLE;
        }
        else
        {

            switch (_phase)
            {
                case Phase.IDLE:
                    if (timeleft <= 0)
                    {
                        target = mgr.playerPosition;
                        _phase = Phase.TELLING;
                        UpdateLine();

                        DisableMovement();

                        lr.startWidth = tellWidth;
                        lr.endWidth = tellWidth;

                        timeleft = tellLength;
                        StartCoroutine(LineFlash());
                    }
                    break;

                case Phase.TELLING:
                    if (timeleft <= 0)
                    {
                        _phase = Phase.ATTACKING;
                        timeleft = attackLength;
                    }
                    break;

                case Phase.ATTACKING:
                    // Update line renderer target
                    lr.startWidth = attackWidth;
                    lr.endWidth = attackWidth;
                    lr.enabled = true;
                    if (timeleft <= 0)
                    {
                        EnableMovement();
                        lr.enabled = false;
                        _phase = Phase.IDLE;
                        timeleft = cooldown;
                    }
                    mgr.DamagePlayer(deltaTime * damagePerSecond);
                    break;

            }
        }

        timeleft -= Time.deltaTime;
    }


    private void UpdateLine()
    {
        lr.SetPositions(new Vector3[] {
            transform.position,
            target
        });
    }

    private void DisableMovement()
    {
        movement.enabled = false;
        hover.enabled = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void EnableMovement()
    {
        movement.enabled = true;
        hover.enabled = true;
        rb.useGravity = true;
    }

    IEnumerator LineFlash()
    {
        while (_phase == Phase.TELLING)
        {
            float t = (timeleft / tellLength) / 2.0f;
            //Debug.Log(t);
            lr.enabled = !lr.enabled;
            yield return new WaitForSeconds(t);
        }
    }

}
