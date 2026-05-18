using UnityEngine;

public enum UnitState
{
    Idle,
    Moving,
    Attacking,
    Dead
}

public class UnitController : MonoBehaviour
{
    [Header("Data")]
    public UnitData data;
    
    [Header("State")]
    [SerializeField] private UnitState currentState;
    [SerializeField] private Lane currentLane;

    private float attackTimer;
    private IDamageable currentTarget;
    private CombatHandler combatHandler;
    private HealthSystem healthSystem;

    public UnitFaction Faction => data.faction;
    public UnitState CurrentState => currentState;
    public Lane CurrentLane => currentLane;

    private void Awake()
    {
        combatHandler = GetComponent<CombatHandler>();
        healthSystem = GetComponent<HealthSystem>();
        
        if (healthSystem != null)
        {
            healthSystem.OnDeath += Die;
        }
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDeath -= Die;
        }
    }

    private void Start()
    {
        if (data != null && currentLane != null)
            Initialize(data, currentLane);
    }

    public void Initialize(UnitData unitData, Lane lane)
    {
        data = unitData;
        currentLane = lane;
        currentState = UnitState.Moving;
        attackTimer = 0f;
        currentTarget = null;
        
        if (healthSystem != null)
        {
            healthSystem.Initialize(data.maxHealth, data.faction);
        }

        if (currentLane != null)
            currentLane.RegisterUnit(this);
            
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentState == UnitState.Dead) return;

        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case UnitState.Moving:
                Move();
                CheckForTargets();
                break;
            case UnitState.Attacking:
                Attack();
                break;
            case UnitState.Idle:
                CheckForTargets();
                break;
        }
    }

    private void Move()
    {
        float direction = (data.faction == UnitFaction.Allied) ? 1f : -1f;
        transform.Translate(Vector3.right * direction * data.movementSpeed * Time.deltaTime);

        // Check if reached target
        if (data.faction == UnitFaction.Allied && transform.position.x >= currentLane.enemyTarget.position.x)
        {
            ReachedEnemySide();
        }
        else if (data.faction == UnitFaction.Enemy && transform.position.x <= currentLane.playerTarget.position.x)
        {
            ReachedPlayerSide();
        }
    }

    private void CheckForTargets()
    {
        if (currentLane == null || combatHandler == null) return;

        currentTarget = combatHandler.ScanForTarget(currentLane, data.attackRange, data.faction);
        if (currentTarget != null)
        {
            currentState = UnitState.Attacking;
        }
    }

    private void Attack()
    {
        if (currentTarget == null || currentTarget.CurrentHealth <= 0 || Vector3.Distance(transform.position, currentTarget.Transform.position) > data.attackRange)
        {
            currentTarget = null;
            currentState = UnitState.Moving;
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= data.attackCooldown)
        {
            combatHandler.PerformAttack(currentTarget, data.attackDamage);
            attackTimer = 0f;
            // Play attack animation
        }
    }

    public void Die()
    {
        if (currentState == UnitState.Dead) return;

        currentState = UnitState.Dead;
        if (currentLane != null)
            currentLane.UnregisterUnit(this);
        
        // Play death animation and then disable/pool
        if (PoolManager.Instance != null && data != null && data.prefab != null)
        {
            PoolManager.Instance.ReturnToPool(data.prefab, gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ReachedEnemySide()
    {
        // Allied unit reached enemy side
        if (ReserveManager.Instance != null && data != null)
        {
            ReserveManager.Instance.AddUnit(data);
        }
        
        // Notify UpgradeManager
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnUnitReachedEnd();
        }

        Die();
    }

    private void ReachedPlayerSide()
    {
        // Enemy unit reached player side
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
        }
        Die();
    }
}
