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

    private GameObject source;
    private SourceType sourceType;
    public void Initialize(WeaponConfig config, GameObject source, SourceType sourceType)
    {
        bulletPrefab = config.bulletPrefab;
        fireForce = config.fireForce;
        fireRate = config.fireRate;

        this.sourceType = sourceType;
        this.source = source;
    }
    public void Fire()
    {
        if (Time.time >= nextFireTime && bulletPrefab != null && firepoint != null)
        {

            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            // Damage Calculation
            var bulletConfig = bullet.GetComponent<Bullet>();


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
