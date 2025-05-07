using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    public LayerMask ignoreLayer;
    [Header("Enemy Parameters")]
    public int experienceToGive;
    public float maxHp;
    public float hp;
    public float damage;

    void Start()
    {
        // Precaution
        if (hp == 0) hp = 10;
        if (experienceToGive == 0) experienceToGive = 10;
        if (damage == 0) damage = 10;
    }
    public void TakeDamage(bool notPlayer = false)
    {
        // Add damage logic
        Destroy(gameObject);
        GameplayManager.Instance.squadLevelManager.GetExperience(experienceToGive);
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        if (!notPlayer) GameplayManager.Instance.spawner.ReportKill(1);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Might consider instead of layermask we use tag
        if (((1 << collision.gameObject.layer) & ignoreLayer) != 0)
        {
            return;
        }
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && player.isActiveAndEnabled)
        {
            // Enemy Damage
            player.TakeDamage(damage);
            Destroy(gameObject);
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
