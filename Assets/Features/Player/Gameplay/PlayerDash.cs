using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private PlayerData playerData;
    [SerializeField] private float lastDashTime;
    [SerializeField] private float dashTimeRemaining;
    [SerializeField] private bool isDashing;
    private Rigidbody2D rb;
    private Vector2 dashDirection;

    public bool IsDashing => isDashing;

    public void Initialize(PlayerData playerData, Rigidbody2D rb)
    {
        this.playerData = playerData;
        this.rb = rb;
    }

    public void HandleDashInput(bool dashPressed, Vector2 moveDirection)
    {
        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
            }
            return;
        }

        if (dashPressed && Time.time >= lastDashTime + playerData.dashCooldown)
        {
            StartDash(moveDirection);
        }
    }
    void FixedUpdate()
    {
        if (isDashing && rb != null)
        {
            rb.velocity = dashDirection * playerData.dashSpeed;
        }
    }
    private void StartDash(Vector2 direction)
    {
        lastDashTime = Time.time;
        dashTimeRemaining = playerData.dashDuration;
        isDashing = true;
        dashDirection = direction.normalized;
    }

}
