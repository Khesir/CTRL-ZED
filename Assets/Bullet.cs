using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask ignoreLayer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is in the ignore layer
        if (((1 << collision.gameObject.layer) & ignoreLayer) != 0)
        {
            // Do nothing – ignore collision
            return;
        }

        // Otherwise, destroy the bullet and maybe do damage
        Destroy(gameObject);

        // Optionally, damage enemy here if tagged or has enemy component
        // Example:
        // var enemy = collision.gameObject.GetComponent<Enemy>();
        // if (enemy != null) enemy.TakeDamage(damage);
    }
}
