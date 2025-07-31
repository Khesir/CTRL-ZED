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

    [SerializeField] private bool inputEnabled = false;

    public void Initialize(IInputService inputService)
    {
        this.inputService = inputService;

        rb = GetComponent<Rigidbody2D>();

        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(rb, playerData.moveSpeed);

        playerDash = GetComponent<PlayerDash>();
        playerDash.Initialize(playerData, rb);
        Debug.Log("[PlayerGameplayService] Succesfully initialized");
    }
    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }
    void Update()
    {
        if (!GameplayManager.Instance.isGameActive || !inputEnabled || inputService == null) return;

        Vector2 moveInput = inputService.MoveInput;
        bool dashPressed = inputService.DashPressed;
        Vector2 mousePos = inputService.GetMouseWorldPosition();

        playerMovement.SetInput(moveInput);
        playerMovement.SetAimTarget(mousePos);
        playerDash.HandleDashInput(dashPressed, moveInput);
    }

}
