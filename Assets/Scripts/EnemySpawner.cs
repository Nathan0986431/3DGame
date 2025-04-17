using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;

    [Header("Spawn Radius Settings")]
    public Transform player;         // Assign Player Transform here
    public float spawnRadius = 20f;  // Max distance from player
    public float minRadius = 5f;     // Optional: avoid spawning too close
    public float heightRange = 5f;   // Enemies can spawn ±height from player Y

    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        Vector3 spawnPosition = GetRandomPositionAroundPlayer();

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;

        // Hook into enemy death to reduce count
        EnemyTarget enemyScript = enemy.GetComponent<EnemyTarget>();
        if (enemyScript != null)
        {
            enemyScript.OnDeath += () => { currentEnemyCount--; };
        }
    }

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

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, spawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minRadius);
    }
}