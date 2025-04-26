using UnityEngine;

public class RandomAmmoSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject ammoPickupPrefab; // Ammo pickup prefab to spawn
    [SerializeField] private float spawnRadius = 10f; // Radius around the player to spawn the pickup
    [SerializeField] private float spawnHeight = 1f;  // Height offset (optional)
    [SerializeField] private float spawnDelay = 2f; // Delay before spawn (optional)

    [Header("Spawn Limits")]
    [SerializeField] private int maxAmmoPickups = 1; // Number of ammo pickups to spawn (default is 1)

    [Header("Player Settings")]
    [SerializeField] private Transform player; // Reference to the player transform

    private bool ammoPickupExists = false; // Track if an ammo pickup already exists in the game

    void Start()
    {
        // Optionally, you can use a delay to spawn it after a few seconds
        Invoke("SpawnAmmoPickup", spawnDelay);
    }

    void SpawnAmmoPickup()
    {
        // Check if an ammo pickup already exists or if the max limit has been reached
        if (!ammoPickupExists && maxAmmoPickups > 0)
        {
            // Get a random position within a radius around the player
            Vector3 randomPosition = player.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius), // Random X position
                spawnHeight, // Fixed Y position (height above ground)
                Random.Range(-spawnRadius, spawnRadius)  // Random Z position
            );

            // Instantiate the ammo pickup prefab at the random position
            Instantiate(ammoPickupPrefab, randomPosition, Quaternion.identity);

            ammoPickupExists = true; // Set the flag to true since a pickup is now in the game
            maxAmmoPickups--; // Decrease the spawn limit
        }
    }

    // Method to call when ammo pickup is destroyed (e.g., when the player picks it up)
    public void AmmoPickupDestroyed()
    {
        ammoPickupExists = false; // Reset the flag, allowing a new ammo pickup to spawn
        maxAmmoPickups++; // Optionally, you can reset the spawn limit if needed (e.g., if you want to allow multiple spawns later)
    }
}
