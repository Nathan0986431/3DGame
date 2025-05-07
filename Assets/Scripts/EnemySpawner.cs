using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject prefab;
        public float spawnInterval = 2f;
    }

    [Header("Enemy Prefabs and Spawn Rates")]
    public EnemySpawnData[] enemyTypes = new EnemySpawnData[4];

    [Header("Spawn Limits")]
    public int maxEnemies = 10;
    private int currentEnemyCount = 0;

    [Header("Spawn Radius Settings")]
    public Transform player;
    public float spawnRadius = 20f;
    public float minRadius = 5f;

    [Header("Map Boundaries")]
    public float mapMinX = -30f;
    public float mapMaxX = 30f;
    public float mapMinZ = -30f;
    public float mapMaxZ = 30f;

    [Header("Height Offset")]
    public float minHeightOffset = 0.3f;
    public float maxHeightOffset = 2f;

    [Header("Terrain")]
    public Terrain terrain;

    void Start()
    {
        // Start a coroutine for each enemy type
        foreach (var enemy in enemyTypes)
        {
            if (enemy.prefab != null)
                StartCoroutine(SpawnEnemyRoutine(enemy));
        }
    }

    IEnumerator SpawnEnemyRoutine(EnemySpawnData enemyData)
    {
        yield return new WaitForSeconds(1f); // Initial delay

        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                Vector3 spawnPos = GetValidSpawnPosition();
                GameObject newEnemy = Instantiate(enemyData.prefab, spawnPos, Quaternion.identity);
                currentEnemyCount++;

                EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.OnDeath += () =>
                    {
                        currentEnemyCount--;
                        StartCoroutine(RespawnEnemy(enemyData, enemyHealth.respawnTime));
                    };
                }
            }

            yield return new WaitForSeconds(enemyData.spawnInterval);
        }
    }

    IEnumerator RespawnEnemy(EnemySpawnData enemyData, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentEnemyCount >= maxEnemies) yield break;

        Vector3 spawnPos = GetValidSpawnPosition();
        GameObject newEnemy = Instantiate(enemyData.prefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;

        EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += () =>
            {
                currentEnemyCount--;
                StartCoroutine(RespawnEnemy(enemyData, enemyHealth.respawnTime));
            };
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 position;
        int attempts = 0;

        do
        {
            Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(minRadius, spawnRadius);
            float spawnX = Mathf.Clamp(player.position.x + offset.x, mapMinX, mapMaxX);
            float spawnZ = Mathf.Clamp(player.position.z + offset.y, mapMinZ, mapMaxZ);

            float terrainHeight = terrain ? terrain.SampleHeight(new Vector3(spawnX, 0f, spawnZ)) : 0f;
            float heightOffset = Random.Range(minHeightOffset, maxHeightOffset);
            float spawnY = terrainHeight + heightOffset;

            position = new Vector3(spawnX, spawnY, spawnZ);
            attempts++;

            if (attempts > 20) break;

        } while (Vector3.Distance(position, player.position) < minRadius);

        return position;
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, spawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(mapMinX, 0f, mapMinZ), new Vector3(mapMaxX, 0f, mapMinZ));
        Gizmos.DrawLine(new Vector3(mapMaxX, 0f, mapMinZ), new Vector3(mapMaxX, 0f, mapMaxZ));
        Gizmos.DrawLine(new Vector3(mapMaxX, 0f, mapMaxZ), new Vector3(mapMinX, 0f, mapMaxZ));
        Gizmos.DrawLine(new Vector3(mapMinX, 0f, mapMaxZ), new Vector3(mapMinX, 0f, mapMinZ));
    }
}
