using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 20;
    public float attackCooldown = 1.5f;
    public LayerMask playerLayer;
    public float soundDistance = 5f; // Distance at which the sound plays

    private float nextAttackTime = 0f;
    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;

    private AudioSource attackSound;
    private bool soundPlayed = false; // To ensure sound only plays once when player enters range

    void Start()
    {
        player = playermanager.instance.Player.transform;
        animator = GetComponentInChildren<Animator>(); // Animator is on the model child
        agent = GetComponent<NavMeshAgent>();

        // Get the AudioSource component attached to the zombie
        attackSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Stop moving and start attacking
            agent.SetDestination(transform.position);
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);

            if (Time.time >= nextAttackTime)
            {
                // Only play sound once when the player gets close (and hasn't already played)
                if (attackSound != null && !soundPlayed)
                {
                    attackSound.Play();
                    soundPlayed = true; // Ensure the sound only plays once
                }

                DealDamage();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            // Chase player
            agent.SetDestination(player.position);
            animator.SetBool("Attack", false);
            animator.SetBool("Walk", true);

            // If player is within the sound range, play sound (only once)
            if (distance <= soundDistance && !soundPlayed)
            {
                if (attackSound != null)
                {
                    attackSound.Play();
                    soundPlayed = true; // Ensure sound is only played once per approach
                }
            }
        }

        // Stop the sound if the player moves away (optional)
        if (distance > soundDistance && soundPlayed)
        {
            attackSound.Stop(); // Stop sound if player moves too far away
            soundPlayed = false; // Reset so sound can play again when player re-enters range
        }
    }

    void DealDamage()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        foreach (Collider playerCollider in hitPlayer)
        {
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
