using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed;
    private Vector2 inputDirection;

    public void Initialize(Rigidbody2D rb, float moveSpeed)
    {
        this.rb = rb;
        this.moveSpeed = moveSpeed;
    }

    public void SetInput(Vector2 input)
    {
        inputDirection = input;
    }

    private void FixedUpdate()
    {
        if (rb == null) return;
        rb.velocity = inputDirection * moveSpeed;
    }
}
