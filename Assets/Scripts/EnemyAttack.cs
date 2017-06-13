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

    private GameManager mgr;
    private LineRenderer lr;

    private enum Phase { IDLE, TELLING, ATTACKING }
    private Phase phase = Phase.IDLE;

    // Use this for initialization
    void Start()
    {
        mgr = GameObject.FindObjectOfType<GameManager>();
        lr = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 playerPosition = mgr.playerPosition;

        float dist = Vector3.Distance(transform.position, playerPosition);
        if (dist > range)
        {
            if (phase != Phase.IDLE) timeleft = cooldown;
            lr.enabled = false;
            phase = Phase.IDLE;
        }
        else
        {
            UpdateLine();

            switch (phase)
            {
                case Phase.IDLE:
                    if (timeleft <= 0)
                    {
                        phase = Phase.TELLING;
                        timeleft = tellLength;
                    }
                    break;

                case Phase.TELLING:
                    // Update line renderer target
                    float t = (tellLength - timeleft) / tellLength;
                    lr.startWidth = Mathf.SmoothStep(0, 2.0f, t);
                    lr.enabled = true;
                    if (timeleft <= 0)
                    {
                        phase = Phase.ATTACKING;
                        timeleft = attackLength;
                    }
                    break;

                case Phase.ATTACKING:
                    // Update line renderer target
                    lr.startWidth = 2.0f;
                    lr.enabled = true;
                    if (timeleft <= 0)
                    {
                        lr.enabled = false;
                        phase = Phase.IDLE;
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
        Vector3 playerPosition = mgr.playerPosition;
        lr.SetPositions(new Vector3[] {
            transform.position,
            playerPosition
        });
    }

}