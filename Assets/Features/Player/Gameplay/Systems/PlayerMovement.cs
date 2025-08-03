using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed;
    private Vector2 inputDirection;
    private Vector2 aimDirection;
    private bool rotateToMouse = false;
    public void Initialize(Rigidbody2D rb, float moveSpeed)
    {
        this.rb = rb;
        this.moveSpeed = moveSpeed;
        Debug.Log("[PlayerMovement] Succesfully initialized");
    }

    public void SetInput(Vector2 input)
    {
        inputDirection = input;
    }
    public void SetAimTarget(Vector2 mouseWorldPosition)
    {
        rotateToMouse = true;
        aimDirection = mouseWorldPosition - rb.position;
    }
    private void FixedUpdate()
    {
        if (rb == null) return;
        // Move
        rb.velocity = inputDirection * moveSpeed;

        // Rotate Toward mouse
        if (rotateToMouse)
        {
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = aimAngle;
        }
    }
}
