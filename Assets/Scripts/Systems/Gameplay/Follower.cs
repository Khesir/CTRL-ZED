using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D targetRb;
    public Rigidbody2D rb;
    public CharacterService characterData;
    public event Action onChangeSupplier;

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
    public LayerMask ignoreLayer;

    public void SetTarget()
    {
        if (controller == null) controller = GetComponent<PlayerController>();

        isControlledPlayer = true;
        controller.enabled = true;
    }
    public void Refresh()
    {
        if (controller == null) controller = GetComponent<PlayerController>();
        if (isControlledPlayer)
        {
            isControlledPlayer = false;
            controller.enabled = false;
        }
        var globalTarget = GameplayManager.Instance.globalTargetPlayer;
        target = globalTarget;
        targetRb = globalTarget.GetComponent<Rigidbody2D>();
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
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
            playerIdleTime = 0f;
            isIdle = false;
        }
        else
        {
            playerIdleTime += Time.fixedDeltaTime;

            if (playerIdleTime >= maxIdleTime)
            {
                if (isIdle)
                {
                    isIdle = false;
                    idleTimer = 0f;
                }
                MoveTowardsPlayer();
                return;
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
                PickNewWanderPosition();
                isIdle = false;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            changeDirectionTimer -= Time.fixedDeltaTime;
            if (changeDirectionTimer <= 0)
            {
                if (UnityEngine.Random.value < 0.5f)
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & ignoreLayer) != 0)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }
    }
}
