using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType; // Reference to the Enemy Type Scriptable Object
    public float attackRange = .2f; // Distance at which the enemy can attack

    private float attackCooldown = 0.5f; // Cooldown between attacks
    private float lastAttackTime; // Time when the last attack occurred

    [SerializeField] private Health enemyHealth; // Reference to the enemy's Health component

    private void Start()
    {
        InitializeEnemy();
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
            // Set health
            enemyHealth = GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.currentHealth = enemyType.health;
                enemyHealth.maxHealth = enemyType.health;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move towards the player with the "PlayerHealth" tag
        GameObject player = GameObject.FindGameObjectWithTag("PlayerHealth");
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemyType.moveSpeed * Time.deltaTime);
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
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Debug.Log("Enemy took " + damage + " damage. Current Health: " + enemyHealth.currentHealth);
            if (enemyHealth.currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log("Enemy destroyed!");
        Destroy(gameObject);
    }
}