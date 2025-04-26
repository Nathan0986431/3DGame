using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public float respawnTime = 5f; // Time before the enemy respawns after death
    public Action OnDeath; // Event that triggers when the enemy dies

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger the OnDeath event
        OnDeath?.Invoke();

        // Disable the enemy's game object to prepare for respawn
        gameObject.SetActive(false);
    }
}