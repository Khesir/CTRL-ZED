using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private CharacterBattleState characterData;

    [SerializeField] private float lastDashTime;
    [SerializeField] private float dashTimeRemaining;
    [SerializeField] private bool isDashing;
    private Rigidbody2D rb;
    private Vector2 dashDirection;

    public bool IsDashing => isDashing;

    public void Initialize(CharacterBattleState characterData, Rigidbody2D rb)
    {
        this.characterData = characterData;
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

        if (dashPressed && Time.time >= lastDashTime + characterData.data.GetInstance().dashCooldown)
        {
            StartDash(moveDirection);
        }
    }
    void FixedUpdate()
    {
        if (isDashing && rb != null)
        {
            rb.velocity = dashDirection * characterData.data.GetInstance().dashSpeed;
        }
    }
    private void StartDash(Vector2 direction)
    {
        lastDashTime = Time.time;
        dashTimeRemaining = characterData.data.GetInstance().dashDuration;
        isDashing = true;
        dashDirection = direction.normalized;
    }

}
