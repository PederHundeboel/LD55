using UnityEngine;

public class OrkController : MonoBehaviour
{
    [SerializeField] private float chargeTime = 2.0f;
    [SerializeField] private float attackRange = 5.0f;
    [SerializeField] private GameObject attackIndicatorPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private float closeEnoughRange = 1.0f;

    private bool isCharging = false;
    private float chargeStartTime;
    private Vector3 attackDirection;

    [SerializeField] private float idleMoveSpeed = 2.0f;
    [SerializeField] private float idleMoveFrequency = 3.0f;
    private float idleMoveTimer = 0;
    private Vector3 idleDirection = Vector3.zero;

    [SerializeField] private float chargeCooldown = 5.0f;
    private float nextChargeTime = 0f;

    private Vector3 lastPosition;
    private float movementThreshold = 0.01f;


    void FixedUpdate()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }
        else
        {
            RandomIdleMovement();
        }
    }

    void MoveTowardsTarget()
    {
        IdleChecker();
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > closeEnoughRange)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float horizontalMove = directionToTarget.x;
            transform.Translate(new Vector3(horizontalMove * Time.deltaTime * 5, 0, 0));
            HandleAnimationAndDirection(horizontalMove);
        }
        else if (!isCharging)
        {
            StartCharge();
        }
    }

    void IdleChecker()
    {
        Vector3 currentPosition = transform.position;
        float distanceMoved = (currentPosition - lastPosition).magnitude;

        if (distanceMoved < movementThreshold)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
        }
        lastPosition = currentPosition;
    }

    void RandomIdleMovement()
    {
        idleMoveTimer -= Time.deltaTime;
        if (idleMoveTimer <= 0)
        {
            idleMoveTimer = idleMoveFrequency;
            idleDirection = (Random.value > 0.5f) ? Vector3.right : Vector3.left;
        }

        transform.Translate(idleDirection * idleMoveSpeed * Time.deltaTime);
        HandleAnimationAndDirection(idleDirection.x);
    }

    void StartCharge()
    {
        if (!isCharging && Time.time >= nextChargeTime)
        {
            isCharging = true;
            chargeStartTime = Time.time;
            attackDirection = (target.position - transform.position).normalized;

            if (attackIndicatorPrefab)
            {
                GameObject indicator = Instantiate(attackIndicatorPrefab, transform.position, Quaternion.identity, transform);
            }

            animator.SetTrigger("charge");
            nextChargeTime = Time.time + chargeCooldown;
        }
    }


    void PerformAttack()
    {
        if (isCharging && Time.time - chargeStartTime >= chargeTime)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("AttackIndicator"))
                {
                    Destroy(child.gameObject);
                }
            }

            Debug.Log("Attack executed towards " + attackDirection);
            isCharging = false;

            animator.SetBool("charge", false);
            animator.SetTrigger("attack");
        }
    }


    void HandleAnimationAndDirection(float moveDirection)
    {
        if (moveDirection > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDirection < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Mathf.Abs(moveDirection) < 0.01f)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
        }
    }
}
