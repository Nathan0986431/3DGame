using UnityEngine;

public class EnemyTarget : MonoBehaviour

{
    public System.Action OnDeath;

    [SerializeField] private float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {health}");

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(); // Notify spawner
        Destroy(gameObject);
    }

}