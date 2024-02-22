using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType; // Reference to the Enemy Type Scriptable Object
    public float attackRange = .2f; // Distance at which the enemy can attack

    private float attackCooldown = 0.5f; // Cooldown between attacks
    private float lastAttackTime; // Time when the last attack occurred
    
    private int currentHealth; // Reference to the enemy's Health component
    
    public float avoidanceRadius = .4f; // Radius within which enemies avoid each other
    public float avoidanceStrength = 1f; // Strength of the avoidance behavior
    public LayerMask enemyLayerMask; // Layer mask to filter out other enemies

    private Rigidbody2D rb; // Reference to the enemy's Rigidbody component
    public SpawnController spawnController;

    private void Start()
    {
        InitializeEnemy();
        spawnController = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<SpawnController>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody component
    }

    private void Update()
    {
        MoveTowardsPlayer();
        if (IsPlayerInRange() && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void InitializeEnemy()
    {
        // Set enemy attributes based on Enemy Type
        if (enemyType != null)
        {  
            currentHealth = enemyType.health;
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move towards the player with the "PlayerHealth" tag while avoiding other enemies
        GameObject player = GameObject.FindGameObjectWithTag("PlayerHealth");
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position;

            // Get all nearby enemies
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, enemyLayerMask);
            
            // Calculate avoidance force from nearby enemies
            Vector3 avoidanceForce = Vector3.zero;
            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                if (enemyCollider.gameObject != gameObject)
                {
                    Vector3 awayFromEnemy = transform.position - enemyCollider.transform.position;
                    avoidanceForce += awayFromEnemy.normalized / awayFromEnemy.sqrMagnitude; // Inverse square law for strength
                }
            }
            
            // Calculate direction towards the player
            Vector3 movementDirection = (targetPosition - transform.position).normalized;
            
            // Set the velocity of the Rigidbody to move towards the player
            rb.velocity = movementDirection * enemyType.moveSpeed;
        }
    }

    private bool IsPlayerInRange()
    {
        // Check if the player is within attack range
        GameObject player = GameObject.FindGameObjectWithTag("PlayerHealth");
        return player != null && Vector3.Distance(transform.position, player.transform.position) <= attackRange;
    }

    private void Attack()
    {
        // Deal damage to the player
        GameObject player = GameObject.FindGameObjectWithTag("PlayerHealth");
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemyType.strength);
        }
        lastAttackTime = Time.time; // Update last attack time
    }

    public void TakeDamage(int damage)
    {
        
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage. Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }   
    }

    private void Die()
    {
        Debug.Log("Enemy destroyed!");
        spawnController.EnemyDestroyed();
        Destroy(gameObject);
    }
}
