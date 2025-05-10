using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPrefab : MonoBehaviour
{
    public AreaWeapon weapon;
    private Vector3 targetSize;
    private float timer;
    public List<Enemy> enemiesInRange;
    public float counter;
    void Start()
    {
        weapon = transform.parent?.GetComponent<AreaWeapon>();
        // Destroy(gameObject, weapon.duration);
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range;
        transform.localScale = Vector3.zero;
        timer = weapon.stats[weapon.weaponLevel].duration;
    }
    void Update()
    {
        // grow and shrink towards targetSize
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            targetSize = Vector3.zero;
            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);
            }
        }
        // Periodic Damage;
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = weapon.stats[weapon.weaponLevel].speed;
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.GetComponent<Enemy>());

            collision.GetComponent<EnemyFollow>().ApplySlow(weapon.stats[weapon.weaponLevel].slowRate);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.GetComponent<Enemy>());
            collision.GetComponent<EnemyFollow>().ClearSlow();
        }
    }
}
