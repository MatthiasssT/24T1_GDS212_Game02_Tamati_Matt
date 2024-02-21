using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 5; // Damage dealt by normal attack
    public float critChance = 0.1f; // Chance of landing a critical hit (10% by default)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Check if the attack lands a critical hit
            bool isCrit = Random.value < critChance;

            // Calculate damage
            int totalDamage = isCrit ? Mathf.RoundToInt(damage * 1.5f) : damage;

            // Deal damage to the enemy
            other.GetComponent<Enemy>().TakeDamage(totalDamage);

            // Log critical hit if applicable
            if (isCrit)
            {
                Debug.Log("Critical Hit! Dealt " + totalDamage + " damage.");
            }
            else
            {
                Debug.Log("Dealt " + totalDamage + " damage.");
            }
        }
    }
}
