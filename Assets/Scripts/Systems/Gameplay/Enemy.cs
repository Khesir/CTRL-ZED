using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    public LayerMask ignoreLayer;
    public void TakeDamage(bool notPlayer = false)
    {
        Destroy(gameObject);
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        if (!notPlayer) GameplayManager.Instance.spawner.ReportKill(1);

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & ignoreLayer) != 0)
        {
            return;
        }
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && player.isActiveAndEnabled)
        {
            player.TakeDamage(1);
            Destroy(gameObject);
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
