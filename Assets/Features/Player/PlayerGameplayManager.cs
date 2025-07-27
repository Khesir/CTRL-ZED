using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayService : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody2D rb;

    // Services    
    private IInputService inputService;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;


    public void Initialize()
    {
        inputService = new InputService(); // Or injected

        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(rb, playerData.moveSpeed);

        playerDash = GetComponent<PlayerDash>();
        playerDash.Initialize(playerData, rb);
        // playerDash.Initialize(playerData.DashConfig);
    }

    void Update()
    {
        Vector2 moveInput = inputService.MoveInput;
        bool dashPressed = inputService.DashPressed;

        playerMovement.SetInput(moveInput);
        playerDash.HandleDashInput(dashPressed, moveInput);
    }
    // public void FixedUpdate()
    // {
    // }
}
