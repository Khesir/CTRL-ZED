using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayService : MonoBehaviour
{
    [SerializeField] private CharacterBattleState characterData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private IInputService inputService;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerCombat playerCombat;
    [SerializeField] private bool inputEnabled = false;

    // Setting Dependencies
    public void SetCharacterData(CharacterBattleState data)
    {
        characterData = data;
    }
    public void SetInputService(IInputService data)
    {
        // We can now change input Service based on cene
        inputService = data;
    }
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        var baseConfig = characterData.data.GetInstance();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(rb, baseConfig.moveSpeed);

        playerDash = GetComponent<PlayerDash>();
        playerDash.Initialize(characterData, rb);

        WeaponConfig weapon = baseConfig.weapon;
        playerCombat = GetComponent<PlayerCombat>();
        playerCombat.Initialize(inputService, weapon, characterData);

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
        playerCombat.TickUpdate();
    }
    public void TakeDamage(float val) => playerCombat.TakeDamage(val);
    public bool IsDead() => characterData.isDead;
    public string GetCharacterID() => characterData.data.GetID();
}
