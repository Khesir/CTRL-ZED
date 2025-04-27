using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float m_Speed = 4f;
    [SerializeField] private float m_RotationSpeed = 8f;
    [SerializeField] private float m_StopDistance = 3f;
    [Header("Boids")]
    [SerializeField] private float m_DetectionDistance = 1f;
    [SerializeField] private float m_SeparationWeight = 1f;
    [SerializeField] private float m_AlignmentWeight = 1f;
    [SerializeField] private float m_CohesionWeight = 1f;


    private Vector2 m_direction = Vector2.zero;
    private float m_MovementSpeedBlend;

    public Transform target; // Assign in Inspector
    public Rigidbody2D rb;
    public float followRange = 5f;       // Distance to start following
    public bool alwaysFollow = false;
    private Vector2 m_SeparationFore = Vector2.zero;
    public LayerMask ignoreLayer;

    private void Start()
    {
        GameplayManager.Instance.switchUser += Refresh;
    }
    private void Refresh()
    {
        target = GameplayManager.Instance.globalTargetPlayer;
    }

    private void Update()
    {
        if (target != null)
        {
            FollowTarget();
        }
    }
    private void FollowTarget()
    {
        m_SeparationFore = Vector2.zero;
        m_direction = target.position - transform.position;
        float distance = m_direction.magnitude;

        var neigbours = GetNeighbours();
        if (neigbours.Length > 0)
        {
            CalculateSeparationForce(neigbours);
            ApplyAllAlignment(neigbours);
            ApplyCohesion(neigbours);
        }
        if (distance > m_StopDistance)
        {
            MoveTowardsTarget();
        }
        else
        {
            StopMove();
        }
        RotateTowardsTarget();
    }

    private void ApplyCohesion(Collider2D[] neigbours)
    {
        Vector2 averagePosition = Vector2.zero;
        foreach (var neigbour in neigbours)
        {
            averagePosition += (Vector2)neigbour.transform.position;
        }
        averagePosition /= neigbours.Length;
        Vector2 cohesionDir = (averagePosition - (Vector2)transform.position).normalized;
        m_SeparationFore += cohesionDir * m_CohesionWeight;
    }

    private void ApplyAllAlignment(Collider2D[] neigbours)
    {
        Vector2 neighboursForward = Vector2.zero;

        foreach (var neigbour in neigbours)
        {
            neighboursForward += (Vector2)neigbour.transform.forward;
        }

        if (neighboursForward != Vector2.zero)
        {
            neighboursForward.Normalize();
        }

        m_SeparationFore += neighboursForward * m_AlignmentWeight;
    }

    private void CalculateSeparationForce(Collider2D[] neigbours)
    {
        // calculate direction to the neigbour
        // calcuate distance from the neighbour
        // calculate opposite vector
        foreach (var neigbour in neigbours)
        {
            Vector2 dir = (Vector2)neigbour.transform.position - (Vector2)transform.position;
            var distance = dir.magnitude;
            var away = -dir.normalized;

            if (distance > 0)
            {
                m_SeparationFore += (away / distance) * m_SeparationWeight;
            }
        }

    }

    private Collider2D[] GetNeighbours()
    {
        var followerMask = LayerMask.GetMask("Enemy");
        return Physics2D.OverlapCircleAll(transform.position, m_DetectionDistance, followerMask);
    }

    private void MoveTowardsTarget()
    {
        m_direction = m_direction.normalized;
        var combinedDirection = (m_direction + m_SeparationFore).normalized;
        Vector2 movement = combinedDirection * m_Speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        m_MovementSpeedBlend = Mathf.Lerp(m_MovementSpeedBlend, 1, Time.deltaTime * m_Speed);
        // Animator blending here (optional)
    }

    private void StopMove()
    {
        m_MovementSpeedBlend = Mathf.Lerp(m_MovementSpeedBlend, 0, Time.deltaTime * m_Speed);
        // Animator blending here (optional)
    }

    private void RotateTowardsTarget()
    {
        if (m_direction.sqrMagnitude > 0.001f) // Avoid NaN errors
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
