using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform firepoint;
    private GameObject bulletPrefab;
    private float fireForce;
    private float fireRate;
    private float nextFireTime = 0f;

    public void Initialize(WeaponConfig config)
    {
        bulletPrefab = config.bulletPrefab;
        fireForce = config.fireForce;
        fireRate = config.fireRate;
        // You could also store baseDamage here if needed
    }
    public void Fire()
    {
        if (Time.time >= nextFireTime && bulletPrefab != null && firepoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firepoint.up * fireForce, ForceMode2D.Impulse);
            }
            bullet.transform.up = firepoint.up;
            nextFireTime = Time.time + (1f / fireRate);
            SoundManager.PlaySound(SoundCategory.Gameplay, SoundType.Gameplay_Shoot);
        }
    }
}
