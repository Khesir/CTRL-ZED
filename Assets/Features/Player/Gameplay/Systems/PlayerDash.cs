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
    private float dashMult = 1;
    public void Initialize(CharacterBattleState characterData, Rigidbody2D rb)
    {
        this.characterData = characterData;
        this.rb = rb;
        Debug.Log("[PlayerDash] Succesfully initialized");

    }

    public void HandleDashInput(bool dashPressed, Vector2 moveDirection, bool overrideDash = false, float dashMult = 5)
    {
        if (overrideDash)
        {
            StartDash(moveDirection, dashMult);
            Debug.Log("Dash Overriden");
            return;
        }
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
            rb.velocity = dashDirection * characterData.data.GetInstance().dashSpeed * dashMult;
        }
    }
    private void StartDash(Vector2 direction, float overrideMult = 1f)
    {
        if (overrideMult == 1f) lastDashTime = Time.time;
        dashTimeRemaining = characterData.data.GetInstance().dashDuration;
        isDashing = true;
        dashDirection = direction.normalized;
        dashMult = overrideMult;
    }
}
