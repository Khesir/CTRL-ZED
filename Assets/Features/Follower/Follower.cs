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
    [Header("References")]
    public Transform player;

    [Header("Follow Settings")]
    public float followDistance = 3f;
    public float moveSpeed = 4f;

    [Header("Attack Settings")]
    public float attackRange = 20f;
    public float attackCooldown = 0.2f;
    public LayerMask enemyLayer;

    private FollowerState currentState = FollowerState.Idle;
    private float lastAttackTime;
    private PlayerGameplayService gameplayService;

    Vector2 idleTarget = Vector2.zero;
    float idleChangeTime = 0f;

    public void Initialize()
    {
        gameplayService = gameObject.GetComponent<PlayerGameplayService>();
        attackRange = 20f;
        followDistance = 5f;
        attackCooldown = 0.09f;
    }

    // 👇 Allow manual target switching
    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
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
        // if it's time to pick a new wander point
        if (Time.time >= idleChangeTime || Vector2.Distance(transform.position, idleTarget) < 0.5f)
        {
            // pick a random direction around player
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;

            // choose a random point within follow range (but not too close)
            float radius = UnityEngine.Random.Range(followDistance * 0.5f, followDistance);
            idleTarget = (Vector2)player.position + randomDir * radius;

            idleChangeTime = Time.time + UnityEngine.Random.Range(5f, 10f); // stay on this point for 2–4s
        }

        // move toward idleTarget
        Vector2 moveDir = (idleTarget - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(moveDir * moveSpeed * 0.5f * Time.deltaTime); // slower than follow speed

        // rotate to face movement direction
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            Time.deltaTime * 5f
        );
    }

    void HandleFollow()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Only move if we're outside followDistance
        if (distance > followDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

            // Rotate towards movement
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10f);
        }
    }

    void HandleAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        // float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // if (distanceToPlayer > followDistance)
        // {
        //     // Too far → move back closer to the player
        //     Vector2 moveDir = (player.position - transform.position).normalized;
        //     transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        //     // Face player while returning
        //     float returnAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
        //     transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, returnAngle), Time.deltaTime * 10f);

        //     return; // Don’t attack until back in range
        // }

        Collider2D enemy = FindNearestEnemyInRange();
        if (enemy != null)
        {
            // 1. Face enemy
            Vector2 direction = (enemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10f);

            // 2. Fire to weapon
            gameplayService?.Fire();

            Debug.Log($"[{name}] Fired at enemy {enemy.name}");

            lastAttackTime = Time.time;
        }
    }
    bool CanSeeEnemyInRange()
    {
        // Vector2 dir = player.position - transform.position;
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, attackRange, enemyLayer);
        return FindNearestEnemyInRange() != null;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followDistance);
    }
}
