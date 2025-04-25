using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform player; // Assign in Inspector
    public Rigidbody2D rb;
    public float moveSpeed = 3f;
    public float followRange = 5f;       // Distance to start following
    public bool alwaysFollow = false;
    private void FixedUpdate()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (alwaysFollow || distance <= followRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Movement towards the player
            rb.velocity = direction * moveSpeed;

            // Rotation to face the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }

    }
}
