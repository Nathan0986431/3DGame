using UnityEngine;
using System;

public class AmmoPickup : MonoBehaviour
{
    [Header("Settings")]
    public int ammoAmount = 5;
    public string playerTag = "Player";

    [Header("Audio")]
    [SerializeField] private AudioSource pickupAudio;

    // Public event to notify spawners or other systems
    public Action OnPickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Try to get the ProjectileGun component
        ProjectileGun gun = other.GetComponentInChildren<ProjectileGun>();
        if (gun != null)
        {
            // Refill ammo and play pickup sound
            gun.RefillAmmo(ammoAmount);
            if (pickupAudio != null)
            {
                pickupAudio.Play();
            }

            // Invoke pickup event for external listeners (e.g., spawner)
            OnPickedUp?.Invoke();

            // Disable collider and hide visuals
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