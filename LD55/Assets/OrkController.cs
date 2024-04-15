using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private HealthContainer healthContainer;
    private Vector3 adjustedTargetPosition;


    private bool isCharging = false;
    private float chargeStartTime;
    private float nextChargeTime = 0f;
    private float idleMoveTimer = 0;
    private Vector3 lastPosition;
    private const float movementThreshold = 0.01f;

    public Action<List<SpellResources.SpellType>> OnHit;

    private List<SpellResources.SpellType> _healthBar;

    public OrkHealthIndicator healthIndicatorPrefab;

    public OrkHealthIndicator healthIndicator;
    private Rigidbody2D rb;

    private void Awake()
    {
        healthContainer = GetComponent<HealthContainer>();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        _healthBar = new List<SpellResources.SpellType>();
        for (int i = 0; i < healthContainer.GetMax(); i++)
        {
            _healthBar.Add((SpellResources.SpellType)Random.Range(0, 3));
        }

        healthIndicator = Instantiate(healthIndicatorPrefab, Vector3.zero, Quaternion.identity, transform);
        healthIndicator.SetBar(_healthBar);

    }

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

    public List<SpellResources.SpellType> GetHealthBar()
    {
        return _healthBar;
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
        Vector2 moveDirection = ((Vector2)adjustedTargetPosition - rb.position).normalized;

        if (!isCharging)
        {
            rb.velocity = moveDirection * idleMoveSpeed;
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
            Vector2 idleDirection = Random.value > 0.5f ? Vector2.right : Vector2.left;
            rb.velocity = idleDirection * idleMoveSpeed;
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

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void HandleAnimationDirection(float horizontalMovement)
    {
        transform.localScale = new Vector3(horizontalMovement > 0 ? 1 : -1, 1, 1);
    }

    public void HitWithSpell(List<SpellResources.SpellType> damage)
    {
        _healthBar.Sort((x, y) => x == SpellResources.SpellType.None ? 1 : 0);

        for (int i = 0; i < _healthBar.Count; i++)
        {
            if (_healthBar[i] != SpellResources.SpellType.None)
            {
                int damageIndex = damage.IndexOf(_healthBar[i]);
                if (damageIndex != -1)
                {
                    _healthBar[i] = SpellResources.SpellType.None;
                    damage.RemoveAt(damageIndex);
                }
            }
        }

        OnHit?.Invoke(_healthBar);

        if (_healthBar.TrueForAll(x => x == SpellResources.SpellType.None))
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
