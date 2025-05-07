using UnityEngine;
using TMPro;
using System.Collections;

public class ProjectileGun : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI ammoText;

    [Header("Gun Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 5f;

    [Header("Ammo")]
    public int maxAmmo = 10;
    private int currentAmmo;

    [Header("Effects")]
    public AudioSource shootAudio;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f;

    // Damage multiplier support
    private float damageMultiplier = 1f;
    private float originalMultiplier = 1f;
    private Coroutine damageBoostCoroutine;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        // Prevent shooting when the game is paused
        if (PauseMenu.Instance != null && PauseMenu.Instance.IsPaused) return;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
            else
            {
                Debug.Log("Out of ammo!");
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Apply the damage multiplier to the bullet
        BulletProjectile bulletScript = bullet.GetComponent<BulletProjectile>();
        if (bulletScript != null)
        {
            bulletScript.SetDamageMultiplier(damageMultiplier);
        }

        currentAmmo--;

        if (shootAudio != null)
            shootAudio.Play();

        if (muzzleFlash != null)
            muzzleFlash.Play();

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void RefillAmmo(int ammoAmount)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammoAmount, maxAmmo);
        Debug.Log("Ammo refilled! Current ammo: " + currentAmmo);
        UpdateAmmoUI();
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
        Debug.Log("Damage multiplier set to: " + multiplier);
    }

    public void ApplyDamageBoost(float multiplier, float duration)
    {
        if (damageBoostCoroutine != null)
        {
            StopCoroutine(damageBoostCoroutine);
        }

        damageBoostCoroutine = StartCoroutine(DamageBoostCoroutine(multiplier, duration));
    }

    private IEnumerator DamageBoostCoroutine(float multiplier, float duration)
    {
        originalMultiplier = damageMultiplier;
        SetDamageMultiplier(multiplier);

        yield return new WaitForSeconds(duration);

        SetDamageMultiplier(originalMultiplier);
    }
}
