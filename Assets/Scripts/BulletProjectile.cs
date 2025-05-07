using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float force = 1000f;
    public float lifetime = 3f;
    public float damage = 25f;

    [Header("Trajectory Visualization")]
    public LineRenderer lineRenderer;
    public int linePoints = 20;

    private Rigidbody rb;

    // Multiplier applied at runtime
    private float damageMultiplier = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Destroy(gameObject, lifetime);

        if (lineRenderer != null)
            lineRenderer.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage * damageMultiplier);

                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoint();
                }

                Destroy(gameObject);
            }
        }
        else
        {
            ReflectBullet(collision);
        }
    }

    void ReflectBullet(Collision collision)
    {
        Vector3 incomingDirection = rb.linearVelocity.normalized;
        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflectedDirection = Vector3.Reflect(incomingDirection, normal);

        if (Vector3.Dot(reflectedDirection, transform.forward) < 0)
            reflectedDirection = -reflectedDirection;

        rb.linearVelocity = reflectedDirection * force;

        if (lineRenderer != null)
            DrawTrajectory(reflectedDirection);
    }

    void DrawTrajectory(Vector3 direction)
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = linePoints;
        Vector3 currentPosition = transform.position;
        lineRenderer.SetPosition(0, currentPosition);

        for (int i = 1; i < linePoints; i++)
        {
            float time = i * 0.1f;
            Vector3 trajectoryPoint = currentPosition + direction * time;
            lineRenderer.SetPosition(i, trajectoryPoint);
        }
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }
}
