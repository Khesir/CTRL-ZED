using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayService : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody2D rb;
    private IInputService inputService;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        inputService = new InputService(); // Replace with injected or test service if needed

        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(rb, playerData.moveSpeed);

        playerDash = GetComponent<PlayerDash>();
        playerDash.Initialize(playerData, rb);
    }

    void Update()
    {
        Vector2 moveInput = inputService.MoveInput;
        bool dashPressed = inputService.DashPressed;

        playerMovement.SetInput(moveInput);
        playerDash.HandleDashInput(dashPressed, moveInput);
    }
}
