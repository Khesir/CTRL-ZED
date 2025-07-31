using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowerState
{
    Idle,
    Follow,
    Attack
}
[RequireComponent(typeof(Collider2D))]
public class Follower : MonoBehaviour
{
    public Rigidbody2D rb;
    public CharacterBattleState characterData;
    [Header("References")]
    public Transform player;

    [Header("Follow Settings")]
    public float followDistance = 3f;
    public float moveSpeed = 4f;

    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public LayerMask enemyLayer;

    private FollowerState currentState = FollowerState.Idle;
    private float lastAttackTime;
    public void Initialize(Transform playerTransform)
    {
        SetTarget(playerTransform);
    }

    // 👇 Allow manual target switching
    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
    }


    public void Initialize(CharacterBattleState data)
    {
        characterData = data;

    }
    void Update()
    {
        if (player == null) return;

        switch (currentState)
        {
            case FollowerState.Idle:
                HandleIdle();
                break;
            case FollowerState.Follow:
                HandleFollow();
                break;
            case FollowerState.Attack:
                HandleAttack();
                break;
        }

        UpdateState();
    }
    void UpdateState()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (CanSeeEnemyInRange())
        {
            currentState = FollowerState.Attack;
        }
        else if (distanceToPlayer > followDistance)
        {
            currentState = FollowerState.Follow;
        }
        else
        {
            currentState = FollowerState.Idle;
        }
    }

    void HandleIdle()
    {
        // Idle logic here (e.g., animation or stand still)
    }

    void HandleFollow()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        // Optional: rotate to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10f);
    }

    void HandleAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        Collider2D enemy = FindNearestEnemyInRange();
        if (enemy != null)
        {
            Debug.Log($"[{name}] Attacked enemy {enemy.name}");
            Kill(enemy.gameObject);
            lastAttackTime = Time.time;
        }
    }
    bool CanSeeEnemyInRange()
    {
        Vector2 dir = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, attackRange, enemyLayer);
        return hit.collider != null;
    }
    Collider2D FindNearestEnemyInRange()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        Collider2D nearest = null;
        float shortestDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                nearest = e;
            }
        }

        return nearest;
    }

    void Kill(GameObject target)
    {
        // Replace with damage/death logic
        Destroy(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followDistance);
    }
}
