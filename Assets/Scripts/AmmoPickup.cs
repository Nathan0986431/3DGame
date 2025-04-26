using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 5;  // Amount of ammo to refill
    public string playerTag = "Player"; // The tag to identify the player
    
    [Header("Audio")]
    [SerializeField] private AudioSource pickupAudio; // Serialized AudioSource for pickup sound
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the ammo pickup is the player
        if (other.CompareTag(playerTag))
        {
            ProjectileGun projectileGun = other.GetComponentInChildren<ProjectileGun>(); // Get the ProjectileGun component
            if (projectileGun != null)
            {
                // Refill ammo by calling RefillAmmo on the ProjectileGun script
                projectileGun.RefillAmmo(ammoAmount);
                
                // Play the pickup sound if assigned
                if (pickupAudio != null)
                {
                    pickupAudio.Play();
                }

                // Call the AmmoPickupDestroyed method to notify the spawner
                RandomAmmoSpawner ammoSpawner = FindObjectOfType<RandomAmmoSpawner>(); // Find the spawner in the scene
                if (ammoSpawner != null)
                {
                    ammoSpawner.AmmoPickupDestroyed();
                }

                // Destroy the ammo pickup after it's used
                Destroy(gameObject);
            }
        }
    }
}