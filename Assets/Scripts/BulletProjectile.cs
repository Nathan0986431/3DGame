using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float force = 1000f;
    public float lifetime = 3f;
    public float damage = 25f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyTarget enemy = collision.gameObject.GetComponent<EnemyTarget>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
