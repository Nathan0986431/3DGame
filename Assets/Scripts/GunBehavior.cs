using UnityEngine;
using TMPro; // Needed for TextMeshPro

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

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
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
                // Optionally: play dry fire sound here
            }
        }
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
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

    // Updated RefillAmmo method to allow partial ammo refill
    public void RefillAmmo(int ammoAmount)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammoAmount, maxAmmo);
        Debug.Log("Ammo refilled! Current ammo: " + currentAmmo);
        UpdateAmmoUI();
    }
}