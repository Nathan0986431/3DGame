using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float force = 1000f; // Bullet's force when shot
    public float lifetime = 3f; // Time before the bullet is destroyed
    public float damage = 25f;  // Damage the bullet deals

    [Header("Trajectory Visualization")]
    public LineRenderer lineRenderer; // LineRenderer for trajectory visualization
    public int linePoints = 20; // Number of points to visualize the trajectory

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Initial force to move the bullet forward
        rb.AddForce(transform.forward * force, ForceMode.Impulse);

        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, lifetime);

        // Enable the LineRenderer to draw the trajectory
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an enemy
        EnemyTarget enemy = collision.gameObject.GetComponent<EnemyTarget>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Reflect the bullet if it hits something, and visualize the new trajectory
        ReflectBullet(collision);
    }

    void ReflectBullet(Collision collision)
    {
        // Get the bullet's current velocity and the surface normal at the point of collision
        Vector3 incomingDirection = rb.linearVelocity.normalized; // Normalize the velocity to avoid issues with inconsistent speeds
        Vector3 normal = collision.contacts[0].normal; // Normal at the point of impact

        // Calculate the reflection direction
        Vector3 reflectedDirection = Vector3.Reflect(incomingDirection, normal);

        // Prevent reverse direction by ensuring the reflected velocity is always forward
        if (Vector3.Dot(reflectedDirection, transform.forward) < 0)
        {
            reflectedDirection = -reflectedDirection; // Flip the direction if it goes backwards
        }

        // Apply the new direction after the reflection
        rb.linearVelocity = reflectedDirection * force; // Keep the original force applied

        // Visualize the new trajectory after reflection
        if (lineRenderer != null)
        {
            DrawTrajectory(reflectedDirection);
        }
    }

    void DrawTrajectory(Vector3 direction)
    {
        // Ensure the LineRenderer is available
        if (lineRenderer == null) return;

        // Set the number of points for the trajectory line
        lineRenderer.positionCount = linePoints;

        // Start at the current bullet position
        Vector3 currentPosition = transform.position;
        lineRenderer.SetPosition(0, currentPosition);

        // Calculate points along the trajectory path (simplified forward motion)
        for (int i = 1; i < linePoints; i++)
        {
            float time = i * 0.1f; // Step through time
            Vector3 trajectoryPoint = currentPosition + direction * time;
            lineRenderer.SetPosition(i, trajectoryPoint);
        }
    }
}
