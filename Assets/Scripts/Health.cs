using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health
    public int currentHealth; // Current health

    public Slider healthSlider; // Reference to the health slider

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage amount

        // Ensure current health doesn't go below 0
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update health UI
        UpdateHealthUI();

        // Check if health has reached 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle death logic here, if needed
        Debug.Log("Health reached 0. Entity died.");
    }

    void UpdateHealthUI()
    {
        // Update the value of the health slider
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }
}

