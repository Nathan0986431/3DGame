using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Respawn Settings")]
    public float respawnTime = 5f; // Optional – use if respawning in place
    public Action OnDeath; // Event to notify spawner or other systems

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage to the enemy.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Handle enemy death.
    /// </summary>
    private void Die()
    {
        // Notify spawner or other systems
        OnDeath?.Invoke();

        // Update the score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoint();
        }

        // Disable the enemy for pooling or cleanup
        gameObject.SetActive(false); // Or use Destroy(gameObject) if not pooling
    }
}