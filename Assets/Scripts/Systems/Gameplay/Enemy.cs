using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    public LayerMask ignoreLayer;
    public void TakeDamage()
    {
        Destroy(gameObject);
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
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
            Debug.Log($"Hit player: {player.gameObject.name}, Enabled: {player.isActiveAndEnabled}, Data: {player.playerData != null}");
            player.TakeDamage(1);
            Destroy(gameObject);
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
