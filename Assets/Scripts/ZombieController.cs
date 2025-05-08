using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    // Reference to the player's transform.
    public Transform player;

    public float range;

    // Reference to the NavMeshAgent component for pathfinding.
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the NavMeshAgent component attached to this object.
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame.
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        // If there's a reference to the player...
        if (player != null && dist <= range)
        {
            // Set the enemy's destination to the player's current position.
            navMeshAgent.SetDestination(player.position);
        }
    }
}
