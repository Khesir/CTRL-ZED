using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D targetRb;
    public Rigidbody2D rb;
    [Header("Wandering Settings")]
    public float normalFollowDistance = 2f;
    public float maxWanderRadius = 3f;
    public float wanderSpeed = 1.5f;
    public float changeDirectionInterval = 10f;

    [Header("Return Settings")]
    public float tooFarDistance = 6f;
    public float returnSpeed = 6f;
    [Header("Rotation Settings")]
    private Vector2 m_direction;
    [SerializeField] private float m_RotationSpeed = 8f;
    private Vector2 wanderTargetPosition;
    private float changeDirectionTimer;
    private bool isIdle = false;
    private float idleTimer = 0f;

    private float playerIdleTime = 0f;
    public float maxIdleTime = 1f;
    public bool isControlledPlayer = false;

    private PlayerController controller;
    private Collider2D collider2d;
    private void Start()
    {
        if (controller == null) controller = GetComponent<PlayerController>();
        if (collider2d == null) collider2d = GetComponent<Collider2D>();

        if (!isControlledPlayer)
        {
            collider2d.enabled = false;
            controller.enabled = false;
            var defaultTarget = GameplayManager.Instance.player;
            target = defaultTarget;
            targetRb = defaultTarget.GetComponent<Rigidbody2D>();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetRb = newTarget.GetComponent<Rigidbody2D>();

        if (controller == null) controller = GetComponent<PlayerController>();
        if (collider2d == null) collider2d = GetComponent<Collider2D>();

        isControlledPlayer = true;
        collider2d.enabled = true;
        controller.enabled = true;
    }
    public void Refresh(Transform newTarget)
    {
        if (controller == null) controller = GetComponent<PlayerController>();
        if (collider2d == null) collider2d = GetComponent<Collider2D>();
        if (isControlledPlayer)
        {
            collider2d.enabled = false;
            isControlledPlayer = false;
            controller.enabled = false;
        }
        target = newTarget;
        targetRb = newTarget.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isControlledPlayer)
        {
            HandleFollow();
        }

    }

    private void HandleFollow()
    {
        if (target == null) return;

        bool isPlayerMoving = targetRb.velocity.magnitude > 0.1f;
        if (isPlayerMoving)
        {
            // Reset idle timer if player is moving
            playerIdleTime = 0f;
            isIdle = false;
        }
        else
        {
            // If player is idle, accumulate idle time
            playerIdleTime += Time.fixedDeltaTime;

            if (playerIdleTime >= maxIdleTime)
            {
                // Player has been idle long enough, follower should move
                if (isIdle)
                {
                    // Stop idling immediately
                    isIdle = false;
                    idleTimer = 0f;
                }
                MoveTowardsPlayer();
                return; // Important: skip wandering when moving toward player
            }
        }

        if (!isPlayerMoving && playerIdleTime < maxIdleTime)
        {
            WanderAroundPlayer();
        }

        if (!isIdle)
        {
            RotateTowardsTarget();
        }
    }
    private void MoveTowardsPlayer()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget > normalFollowDistance)
        {
            // Calculate dynamic speed
            float minSpeed = returnSpeed * 0.5f;
            float maxSpeed = returnSpeed;
            float maxFollowDistance = tooFarDistance * 2f;

            float speedFactor = Mathf.Clamp01(distanceToTarget / maxFollowDistance);
            float dynamicSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedFactor);
            MoveTowards(target.position, dynamicSpeed);
            isIdle = false;
        }
        else
        {
            WanderAroundPlayer();
        }
    }
    private void WanderAroundPlayer()
    {
        if (isIdle)
        {
            idleTimer -= Time.fixedDeltaTime;
            if (idleTimer <= 0f)
            {
                // Done idling, start moving again
                PickNewWanderPosition();
                isIdle = false;
            }
            else
            {
                // While idling, stay still
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            changeDirectionTimer -= Time.fixedDeltaTime;
            if (changeDirectionTimer <= 0)
            {
                // Instead of always picking new move, sometimes start idling
                if (UnityEngine.Random.value < 0.5f) // 50% chance to idle
                {
                    StartIdling();
                }
                else
                {
                    PickNewWanderPosition();
                }
            }

            MoveTowards(wanderTargetPosition, wanderSpeed);
        }
    }
    private void StartIdling()
    {
        isIdle = true;
        idleTimer = UnityEngine.Random.Range(5f, 10f);
        rb.velocity = Vector2.zero;
    }
    private void MoveTowards(Vector2 destination, float speed)
    {
        m_direction = (destination - (Vector2)transform.position).normalized;
        rb.velocity = Vector2.Lerp(rb.velocity, m_direction * speed, 0.1f);
    }

    private void PickNewWanderPosition()
    {
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * maxWanderRadius;
        wanderTargetPosition = (Vector2)target.position + randomOffset;
        changeDirectionTimer = changeDirectionInterval;
    }
    private void RotateTowardsTarget()
    {
        if (m_direction.sqrMagnitude > 1f) // Avoid NaN errors
        {
            float angle = Mathf.Atan2(m_direction.y, m_direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, Time.deltaTime * m_RotationSpeed);
        }
    }

}
