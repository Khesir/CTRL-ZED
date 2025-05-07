using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public weapon weapon;
    public CharacterService playerData;
    Vector2 moveDirection;
    Vector2 mousePosition;

    [Header("Dash Control")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTime;
    private float lastDashTime;

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButton(0))
        {
            weapon.Fire();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = moveDirection * dashSpeed;
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0f)
            {
                EndDash();
            }
        }
        else
        {
            rb.velocity = moveDirection * moveSpeed;
        }

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        lastDashTime = Time.time;
    }

    private void EndDash()
    {
        isDashing = false;
    }
    public void TakeDamage(float damage)
    {
        playerData.TakeDamage(damage);
        if (playerData.IsDead())
        {
            gameObject.SetActive(false);
            var follower = gameObject.GetComponent<Follower>();
            if (follower != null)
            {
                follower.enabled = false;
            }

            var index = GameplayManager.Instance.IsAvailable();
            if (index != -1)
            {
                GameplayManager.Instance.SwitchControlledFollower(index);
            }
            else
            {
                Debug.Log("Game Over");
            }
        }
    }
}
