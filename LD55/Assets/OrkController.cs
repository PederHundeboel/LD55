using UnityEngine;

public class OrkController : MonoBehaviour
{
    [SerializeField] private float chargeTime = 2.0f;
    [SerializeField] private GameObject attackIndicatorPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private float closeEnoughRange = 1.0f;
    [SerializeField] private float yLevelThreshold = 0.1f;
    [SerializeField] private float idleMoveSpeed = 2.0f;
    [SerializeField] private float idleMoveFrequency = 3.0f;
    [SerializeField] private float chargeCooldown = 5.0f;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, -0.5f, 0);
    private Vector3 adjustedTargetPosition;


    private bool isCharging = false;
    private float chargeStartTime;
    private float nextChargeTime = 0f;
    private float idleMoveTimer = 0;
    private Vector3 lastPosition;
    private const float movementThreshold = 0.01f;

    void FixedUpdate()
    {
        if (target != null)
        {
            ProcessTargetInteraction();
        }
        else
        {
            ExecuteIdleBehavior();
        }
    }

    private void ProcessTargetInteraction()
    {
        adjustedTargetPosition = target.position + targetOffset;
        UpdateIdleState();

        if (ShouldMoveTowardsTarget())
        {
            MoveTowardsTarget();
        }
        else if (ShouldStartCharge())
        {
            StartCharge();
        }
        isCharging = false;

    }

    private bool ShouldMoveTowardsTarget()
    {
        return Vector3.Distance(transform.position, adjustedTargetPosition) > closeEnoughRange || Mathf.Abs(transform.position.y - adjustedTargetPosition.y) >= yLevelThreshold;
    }

    private void MoveTowardsTarget()
    {
        Vector3 moveDirection = (adjustedTargetPosition - transform.position).normalized;

        if (!isCharging)
        {
            transform.Translate(moveDirection * idleMoveSpeed * Time.deltaTime);
            HandleAnimationDirection(moveDirection.x);
        }
    }


    private bool ShouldStartCharge()
    {
        return !isCharging && Time.time >= nextChargeTime;
    }

    private void UpdateIdleState()
    {
        Vector3 currentPosition = transform.position;
        if (Vector3.Distance(currentPosition, lastPosition) < movementThreshold)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
        }
        lastPosition = currentPosition;
    }

    private void ExecuteIdleBehavior()
    {
        idleMoveTimer -= Time.deltaTime;
        if (idleMoveTimer <= 0)
        {
            idleMoveTimer = idleMoveFrequency;
            Vector3 idleDirection = Random.value > 0.5f ? Vector3.right : Vector3.left;
            transform.Translate(idleDirection * idleMoveSpeed * Time.deltaTime);
            HandleAnimationDirection(idleDirection.x);
        }
    }

    private void StartCharge()
    {
        isCharging = true;
        chargeStartTime = Time.time;
        Instantiate(attackIndicatorPrefab, transform.position, Quaternion.identity, transform);
        animator.SetTrigger("charge");
        nextChargeTime = Time.time + chargeCooldown;
    }


    private void HandleAnimationDirection(float horizontalMovement)
    {
        transform.localScale = new Vector3(horizontalMovement > 0 ? 1 : -1, 1, 1);
    }
    
    public void Kill()
    {
        Destroy(gameObject);
    }
}
