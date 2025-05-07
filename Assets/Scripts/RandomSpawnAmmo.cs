using UnityEngine;
using System.Collections; // This is required to use IEnumerator

public class RandomAmmoSpawner : MonoBehaviour
{
    [Header("Ammo Pickup Settings")]
    public GameObject ammoPickupPrefab;
    public float spawnInterval = 10f;
    [SerializeField] private int maxAmmoPickups = 1;
    [SerializeField] private float despawnTime = 5f; // Time before despawning the ammo pickup

    [Header("Spawn Area Bounds")]
    public Vector3 areaCenter;
    public Vector3 areaSize;

    [Header("Spawn Height Range")]
    [SerializeField] private float minHeight = 0f;
    [SerializeField] private float maxHeight = 3f;

    private int currentPickupCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnAmmoPickup), spawnInterval, spawnInterval);
    }

    void SpawnAmmoPickup()
    {
        if (currentPickupCount >= maxAmmoPickups) return;

        Vector3 spawnPos = GetRandomPositionWithinArea();

        GameObject newPickup = Instantiate(ammoPickupPrefab, spawnPos, Quaternion.identity);
        currentPickupCount++;

        // Attach callback to notify this spawner when it's collected
        AmmoPickup pickup = newPickup.GetComponent<AmmoPickup>();
        if (pickup != null)
        {
            pickup.OnPickedUp += AmmoPickupDestroyed;
        }

        // Start a coroutine to despawn the pickup after the specified time
        StartCoroutine(DespawnPickup(newPickup)); // Fixed: use StartCoroutine with the method correctly
    }

    // Despawn the pickup after a set amount of time
    private IEnumerator DespawnPickup(GameObject pickup)
    {
        // Wait for the despawn time to elapse
        yield return new WaitForSeconds(despawnTime);

        // Destroy the pickup game object
        if (pickup != null)
        {
            Destroy(pickup);
        }

        currentPickupCount--;
    }

    public void AmmoPickupDestroyed()
    {
        currentPickupCount--;
    }

    Vector3 GetRandomPositionWithinArea()
    {
        Vector3 randomPos = new Vector3(
            areaCenter.x + Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(minHeight, maxHeight), // Constrain height
            areaCenter.z + Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
        );

        return randomPos;
    }

    // Optional: visualize spawn bounds in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
