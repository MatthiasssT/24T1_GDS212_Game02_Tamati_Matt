using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    public GameObject basicEnemyPrefab; // Reference to the BasicEnemy prefab
    public GameObject agileEnemyPrefab; // Reference to the AgileEnemy prefab
    public GameObject tankEnemyPrefab; // Reference to the TankEnemy prefab

    public Vector2 spawnAreaCenter; // Center of the spawn area
    public float spawnAreaWidth = 10f; // Width of the spawn area
    public float spawnAreaHeight = 5f; // Height of the spawn area

    [SerializeField] private int totalEnemiesSpawned = 0; // Total number of enemies spawned
    private int basicEnemyCount = 3; // Initial number of BasicEnemies to spawn
    private int agileEnemyCount = 0; // Number of AgileEnemies to spawn
    private int tankEnemyCount = 0; // Number of TankEnemies to spawn


    public void StartNextRound()
    {
        totalEnemiesSpawned = 0;    
        int currentRound = gameManager.GetCurrentRound();
        if (currentRound > 0) // Only start spawning enemies after round 0
        {
            SpawnEnemiesForRound(currentRound);
        }
    }

    void SpawnEnemiesForRound(int roundNumber)
    {
        basicEnemyCount = CalculateBasicEnemyCountForRound(roundNumber);
        agileEnemyCount = CalculateAgileEnemyCountForRound(roundNumber);
        tankEnemyCount = CalculateTankEnemyCountForRound(roundNumber);

        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        for (int i = 0; i < basicEnemyCount; i++)
        {
            SpawnEnemy(basicEnemyPrefab);
            yield return new WaitForSeconds(1f); // Adjust spawn delay as needed
        }

        for (int i = 0; i < agileEnemyCount; i++)
        {
            SpawnEnemy(agileEnemyPrefab);
            yield return new WaitForSeconds(1f); // Adjust spawn delay as needed
        }

        for (int i = 0; i < tankEnemyCount; i++)
        {
            SpawnEnemy(tankEnemyPrefab);
            yield return new WaitForSeconds(1f); // Adjust spawn delay as needed
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Randomly choose a position within the spawn area
        Vector2 spawnPosition = GetValidSpawnPosition();

        if (spawnPosition != Vector2.zero)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            totalEnemiesSpawned++;
            Debug.Log("Spawned: " + newEnemy.name + " at " + spawnPosition);
        }
    }

    Vector2 GetValidSpawnPosition()
    {
        const int maxAttempts = 20; // Maximum number of attempts to find a valid spawn position

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaCenter.x - spawnAreaWidth / 2, spawnAreaCenter.x + spawnAreaWidth / 2),
                Random.Range(spawnAreaCenter.y - spawnAreaHeight / 2, spawnAreaCenter.y + spawnAreaHeight / 2)
            );

            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 1f); // Check for colliders within 1 unit radius

            // Check if the spawn position is valid (not too close to other enemies)
            if (colliders.Length == 0)
            {
                return spawnPosition;
            }
        }

        // If no valid spawn position is found after maxAttempts, return zero vector
        return Vector2.zero;
    }

    int CalculateBasicEnemyCountForRound(int roundNumber)
    {
        if (roundNumber % 2 != 0 || roundNumber >= 10)
        {
            return basicEnemyCount + (roundNumber / 2) * 2;
        }
        else
        {
            return basicEnemyCount;
        }
    }

    int CalculateAgileEnemyCountForRound(int roundNumber)
    {
        if (roundNumber >= 5 && roundNumber % 5 == 0)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    int CalculateTankEnemyCountForRound(int roundNumber)
    {
        if (roundNumber >= 10 && roundNumber % 10 == 0)
        {
            return 2 + (roundNumber / 10);
        }
        else
        {
            return 0;
        }
    }

    public void EnemyDestroyed()
    {
        totalEnemiesSpawned--;
        if (totalEnemiesSpawned <= 0)
        {
            // All enemies are destroyed, start the next round
            Debug.Log("Round is over!");
            gameManager.StartRound();
        }
    }
}
