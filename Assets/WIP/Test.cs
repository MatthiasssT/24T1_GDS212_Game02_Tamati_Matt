using UnityEngine;

public class Test : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the object
    public int currentHealth; // Current health of the object

    // Event for when the object's health reaches zero
    public delegate void OnDeath();
    public event OnDeath DeathEvent;

    void Start()
    {
        currentHealth = 75; // Initialize current health to max health
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage amount

        // Check if health has reached zero
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Ensure health doesn't go negative
            Die(); // Call Die method
        }
    }

    // Method to heal
    public void Heal(int amount)
    {
        currentHealth += amount; // Increase current health by the heal amount

        // Ensure current health doesn't exceed max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    // Method to handle death
    private void Die()
    {
        // Fire the DeathEvent if there are any subscribers
        DeathEvent?.Invoke();

        // Destroy or deactivate the object
        gameObject.SetActive(false);
        // Alternatively, you can destroy the GameObject using Destroy(gameObject)
    }
}
