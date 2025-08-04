using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public Rigidbody2D rb;
    public float lifetime = 5f;
    public float speed = 5;
    private Vector2 moveDirection;
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifetime); // Still destroy after a few seconds
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is in the ignore layer
        if (((1 << collision.gameObject.layer) & ignoreLayer) != 0)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());

            return;
        }

        Destroy(gameObject);

        var enemy = collision.gameObject.GetComponent<EnemyService>();
        if (enemy != null) enemy.TakeDamage(100);
    }
}
