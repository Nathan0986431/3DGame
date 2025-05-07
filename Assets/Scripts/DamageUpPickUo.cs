using UnityEngine;
using System;

public class DamagePickup : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";
    public float damageMultiplier = 2f;
    public float duration = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource pickupAudio; // Drag the AudioSource component here

    // Public event to notify spawners or other systems
    public Action OnPickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Try to get the ProjectileGun component
            ProjectileGun gun = other.GetComponentInChildren<ProjectileGun>();
            if (gun != null)
            {
                // Apply the damage boost to the player's gun
                gun.ApplyDamageBoost(damageMultiplier, duration);

                // Play pickup sound
                if (pickupAudio != null)
                {
                    pickupAudio.Play();
                }

                // Invoke pickup event for external listeners (e.g., spawner)
                OnPickedUp?.Invoke();

                // Hide visuals and disable collider
                GetComponent<Collider>().enabled = false;
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }

                // Destroy the object after the sound has played
                Destroy(gameObject, pickupAudio != null ? pickupAudio.clip.length : 0f);
            }
        }
    }
}