using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;    // The enemy prefab to spawn
    public float spawnInterval = 2f;  // Time between enemy spawns
    public int maxEnemies = 10;       // Maximum number of enemies allowed at once

    [Header("Spawn Radius Settings")]
    public Transform player;          // The player's transform (used for spawn radius)
    public float spawnRadius = 20f;   // Max distance from player
    public float minRadius = 5f;      // Avoid spawning too close to the player
    public float heightRange = 5f;    // Enemies spawn ±height from player Y

    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);  // Start spawning at regular intervals
    }

    void SpawnEnemy()
    {
        // Check if we are at the max enemy limit
        if (currentEnemyCount >= maxEnemies) return;

        // Get a random spawn position around the player
        Vector3 spawnPosition = GetRandomPositionAroundPlayer();

        // Spawn the enemy at the random position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;

        // Get the enemy's health script (if it exists)
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            // Hook into the enemy's OnDeath event to reduce the enemy count when it dies
            enemyHealth.OnDeath += () => 
            { 
                currentEnemyCount--; 
                StartCoroutine(RespawnEnemy(enemyHealth)); // Start respawn logic
            };
        }
    }

    // Coroutine to handle enemy respawn after a certain delay
    private IEnumerator RespawnEnemy(EnemyHealth enemyHealth)
    {
        // Wait for the respawn time (same time as set in the enemy's health script)
        yield return new WaitForSeconds(enemyHealth.respawnTime);

        // Respawn the enemy
        Vector3 spawnPosition = GetRandomPositionAroundPlayer(); // Get a new random spawn position
        enemyHealth.gameObject.SetActive(false); // Disable the enemy for respawn
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++; // Increase the current enemy count

        // Optionally, reinitialize the enemy's health if needed
        EnemyHealth newEnemyHealth = newEnemy.GetComponent<EnemyHealth>();
        if (newEnemyHealth != null)
        {
            newEnemyHealth.OnDeath += () => 
            { 
                currentEnemyCount--; 
                StartCoroutine(RespawnEnemy(newEnemyHealth)); // Start respawn logic for new enemy
            };
        }
    }

    // Get a random spawn position around the player
    Vector3 GetRandomPositionAroundPlayer()
    {
        Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(minRadius, spawnRadius);
        float heightOffset = Random.Range(-heightRange, heightRange);

        Vector3 spawnPos = new Vector3(
            player.position.x + circle.x,
            player.position.y + heightOffset,
            player.position.z + circle.y
        );

        return spawnPos;
    }

    // Draw spawn areas in the editor (for debugging)
    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, spawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minRadius);
    }
}
