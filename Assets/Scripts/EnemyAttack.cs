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

    // Attack cooldown
    public float cooldown = 5.0f;
    private float timeleft = 0.0f;

    public float tellWidth = 1.0f;
    public float attackWidth = 3.0f;

    private GameObject player;
    private GameManager mgr;
    private LineRenderer lr;
    private Vector3 targetPosition;
    private Vector3 targetOffset;
    private Vector3 targettingVelocity = Vector3.zero;
    //public float attackFollowRate = 0.5f;
    public PulsingGradient pulsingGradient;

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

    void SetPhase(Phase p)
    {
        _phase = p;
        switch (_phase)
        {
            case Phase.TELLING:
                //DisableMovement();
                timeleft = tellLength;

                targetPosition = mgr.playerPosition;
                targetOffset = Random.insideUnitCircle * 1;
                targettingVelocity = Vector3.zero;

                //StartCoroutine(LineFlash());
                break;

            case Phase.ATTACKING:
                timeleft = attackLength;
                break;

            case Phase.IDLE:
                EnableMovement();
                timeleft = cooldown;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        timeleft -= Time.deltaTime;

        Vector3 playerPosition = mgr.playerPosition;
        float dist = Vector3.Distance(transform.position, playerPosition);
        float currentRange = range; //(_phase == Phase.IDLE ? range : range * 2);
        if (dist > currentRange)
        {
            lr.enabled = false;
            if (_phase != Phase.IDLE)
            {
                SetPhase(Phase.IDLE);
            }
        }
        else
        {
            switch (_phase)
            {
                case Phase.IDLE:
                    lr.enabled = false;
                    if (timeleft <= 0)
                    {
                        SetPhase(Phase.TELLING);
                    }
                    break;

                case Phase.TELLING:
                    UpdateTargetPosition(playerPosition);
                    UpdateLinePositions(currentRange);
                    lr.startWidth = tellWidth;
                    lr.endWidth = tellWidth;
                    lr.enabled = true;

                    float t = (tellLength - timeleft) / tellLength;
                    pulsingGradient.EdgeMultiplierFreq = Mathf.Lerp(1, 6, t);
                    pulsingGradient.EdgeMultiplierMin = Mathf.Lerp(8, 2, t);
                    pulsingGradient.EdgeMultiplierMax = Mathf.Lerp(10, 4, t);
                    pulsingGradient.MinColor = new Color(0, t, 0);
                    pulsingGradient.MaxColor = new Color(t, 1, 0);

                    if (timeleft <= 0)
                    {
                        SetPhase(Phase.ATTACKING);
                    }
                    break;

                case Phase.ATTACKING:
                    UpdateTargetPosition(playerPosition);
                    UpdateLinePositions(currentRange);
                    lr.startWidth = attackWidth;
                    lr.endWidth = attackWidth;
                    lr.enabled = true;
                    
                    pulsingGradient.EdgeMultiplierFreq = 6;
                    pulsingGradient.EdgeMultiplierMin = 2;
                    pulsingGradient.EdgeMultiplierMax = 4;
                    pulsingGradient.MinColor = new Color(0, 1, 0);
                    pulsingGradient.MaxColor = new Color(1, 1, 0);

                    if (timeleft <= 0)
                    {
                        SetPhase(Phase.IDLE);
                    }
                    break;

            }
        }
    }

    private void UpdateTargetPosition(Vector3 playerPosition)
    {
        // Lag target behind player
        float smoothTime = (_phase == Phase.ATTACKING ? 1.0f : 0.2f);
        targetPosition = Vector3.SmoothDamp(targetPosition, playerPosition + targetOffset, ref targettingVelocity, smoothTime, 10, Time.deltaTime);
    }
    
    private void UpdateLinePositions(float currentRange)
    {
        Vector3 enemyPos = transform.position;
        Vector3 dir = (targetPosition - enemyPos).normalized;
        int layerMask = (1 << LayerMask.NameToLayer("Planet")) | (1 << LayerMask.NameToLayer("Player"));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, currentRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            lr.SetPositions(new Vector3[] { enemyPos, hit.point });
            if (string.Equals(hit.collider.tag, "Player"))
            {
                mgr.DamagePlayer(Time.deltaTime * damagePerSecond);
            }
        }
        else
        {
            lr.SetPositions(new Vector3[] { enemyPos, enemyPos + dir * currentRange });
        }
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

    //IEnumerator LineFlash()
    //{
    //    while (_phase == Phase.TELLING)
    //    {
    //        float t = (timeleft / tellLength) / 2.0f;
    //        lr.enabled = !lr.enabled;
    //        yield return new WaitForSeconds(t);
    //    }
    //}

}
