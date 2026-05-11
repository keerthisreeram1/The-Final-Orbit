using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] float detectionRange = 15f;
    [SerializeField] float attackRange = 2f;

    [Header("Patrol")]
    [SerializeField] float patrolRadius = 10f;
    [SerializeField] float patrolWaitTime = 2f;

    [Header("Combat")]
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackCooldown = 1.5f;

    [Header("Speed")]
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float chaseSpeed = 5f;

    enum State { Patrol, Chase, Attack }
    State currentState = State.Patrol;

    NavMeshAgent agent;
    Transform player;
    float attackTimer = 0f;
    float patrolTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("EnemyAI: No GameObject tagged 'Player' found!");
        }

        SetNewPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Decide state
        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Patrol;
        }

        // Run state
        switch (currentState)
        {
            case State.Patrol: HandlePatrol(); break;
            case State.Chase: HandleChase(); break;
            case State.Attack: HandleAttack(); break;
        }
    }

    void HandlePatrol()
    {
        agent.speed = patrolSpeed;
        patrolTimer += Time.deltaTime;

        if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolPoint();
                patrolTimer = 0f;
            }
        }
    }

    void HandleChase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void HandleAttack()
    {
        agent.SetDestination(transform.position); // Stop moving
        transform.LookAt(player);                 // Face the player

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            DealDamageToPlayer();
            attackTimer = 0f;
        }
    }

    void DealDamageToPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = player.GetComponentInChildren<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Enemy attacked! Player health: ");
        }
        else
        {
            Debug.LogWarning("EnemyAI: PlayerHealth script not found on player!");
        }
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            if (agent.isOnNavMesh)
                agent.SetDestination(hit.position);
        }
    }

    // Shows detection range (yellow) and attack range (red) in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}