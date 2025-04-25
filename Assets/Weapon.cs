using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepoint;
    public float fireForce = 20f;
    public float nextFireTime = 0f;
    public float fireRate = 5f;
    public void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.up * fireForce, ForceMode2D.Impulse);
            nextFireTime = Time.time + (1f / fireRate);
        }
    }
}
