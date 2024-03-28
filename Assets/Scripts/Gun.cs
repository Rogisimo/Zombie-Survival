using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 10;
    private int currentAmmo;
    public float damage = 10f;
    public float range = 100f;
    public GameObject firePoint;
    public GameObject fireParticles;
    public float fireRate = 2f; // Shots per second
    private float nextTimeToFire = 0f;
    public Camera fpsCam;
    public TextMeshProUGUI ammoDisplay;
    public LayerMask enemyLayers;
    public float reloadTime = 2f; // Time it takes to reload in seconds
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
    }

    void Update()
    {
        if (isReloading)
            return;

        // Check for reload input, for example pressing 'R'
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        // Check if the fire button is held down and the current time is past the next allowed fire time
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        UpdateAmmoDisplay();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        ammoDisplay.text = "Reloading...";
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoDisplay();
    }

    void Shoot()
    {
        if (isReloading)
            return;

        if (currentAmmo > 0)
        {
            Instantiate(fireParticles, firePoint.transform.position, Quaternion.identity);
            currentAmmo--;
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, enemyLayers))
            {
                Debug.Log(hit.transform.name + " hit.");
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            else
            {
                Debug.Log("Missed!");
            }
        }
        else
        {
            Debug.Log("Out of Ammo!");
        }
    }

    void UpdateAmmoDisplay()
    {
        if (!isReloading)
        {
            ammoDisplay.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
        }
    }
}
