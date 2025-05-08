using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ImprovedEnemyScript : MonoBehaviour
{
    // Player detection
    public float lookRadius = 10f;
    
    // Wandering behavior
    public float wanderRadius = 5f;
    public float minWanderTime = 3f;
    public float maxWanderTime = 8f;
    private Vector3 startingPosition;
    private bool isWandering = false;
    private float wanderTimer;
    
    // References
    private Transform target;
    private NavMeshAgent agent;
    
    // States
    private enum EnemyState
    {
        Idle,
        Wandering,
        Chasing
    }
    private EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        target = playermanager.instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        SetNewWanderTime();
    }

    void Update()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
        
        // Determine state based on player distance
        if (distanceToPlayer <= lookRadius)
        {
            // Player is nearby - chase them
            currentState = EnemyState.Chasing;
            ChasePlayer();
        }
        else
        {
            // Player is not nearby - wander or idle
            if (currentState == EnemyState.Chasing)
            {
                // Just lost sight of player, go back to idle
                currentState = EnemyState.Idle;
                agent.ResetPath();
            }
            
            // Handle wandering behavior
            HandleWandering();
        }
    }
    
    void HandleWandering()
    {
        // If we're not already wandering, count down the timer
        if (!isWandering)
        {
            wanderTimer -= Time.deltaTime;
            
            // Time to start wandering
            if (wanderTimer <= 0)
            {
                StartWandering();
            }
        }
        // If wandering and reached destination, go back to idle
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWandering = false;
            currentState = EnemyState.Idle;
            SetNewWanderTime();
        }
    }
    
    void StartWandering()
    {
        isWandering = true;
        currentState = EnemyState.Wandering;
        
        // Find a random point within wanderRadius of start position
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += startingPosition;
        
        NavMeshHit hit;
        // Sample a valid position on the NavMesh closest to the random point
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    void SetNewWanderTime()
    {
        // Set a random time before wandering again
        wanderTimer = Random.Range(minWanderTime, maxWanderTime);
    }

    void ChasePlayer()
    {
        // Chase player
        agent.SetDestination(target.position);
        
        // If close enough, face the player
        if (Vector3.Distance(target.position, transform.position) <= agent.stoppingDistance)
        {
            FaceTarget();
            
            // Attack logic could go here
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        // Draw player detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        
        // Draw wandering radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
    
    // Add this method to make enemy take damage (compatible with your PlayerAttack script)
    public void TakeDamage(int damage)
    {
        // Implement damage logic here
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        
        // Example:
        // health -= damage;
        // if (health <= 0) Die();
    }
}