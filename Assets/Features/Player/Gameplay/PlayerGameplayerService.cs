using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayService : MonoBehaviour, IStatHandler, IDamageable
{
    [SerializeField] private CharacterBattleState characterData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private IInputService inputService;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerCombat playerCombat;
    [SerializeField] private bool inputEnabled = false;
    public bool isControlled = false;

    public bool isDead => characterData.isDead;

    public void Initialize()
    {
        // For now Its we'll pass the characterGameState rather than passing just the needed value.
        // Just for convinience
        rb = GetComponent<Rigidbody2D>();
        var baseConfig = characterData.data.GetInstance();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(rb, baseConfig.moveSpeed);

        playerDash = GetComponent<PlayerDash>();
        playerDash.Initialize(characterData, rb);

        playerCombat = GetComponent<PlayerCombat>();
        playerCombat.Initialize(inputService, characterData);

        characterData.onDeath += HandleDeath;

        Debug.Log("[PlayerGameplayService] Succesfully initialized");
    }
    public void SetInputEnabled(bool enabled)
    {
        // This differenciates the controlled player and the manager
        inputEnabled = enabled;
        isControlled = enabled;
    }
    void Update()
    {
        if (!GameplayManager.Instance.isGameActive || !inputEnabled || inputService == null || isDead) return;

        Vector2 moveInput = inputService.MoveInput;
        bool dashPressed = inputService.DashPressed;
        Vector2 mousePos = inputService.GetMouseWorldPosition();
        playerMovement.SetInput(moveInput);
        playerMovement.SetAimTarget(mousePos);
        playerDash.HandleDashInput(dashPressed, moveInput);

        playerCombat.TickUpdate();
        playerCombat.HandleSkillInput();
    }

    // Setting Dependencies
    public void SetCharacterData(CharacterBattleState data)
    {
        characterData = data;
    }
    public void SetInputService(IInputService data)
    {
        // We can now change input Service based on scene
        inputService = data;
    }
    public void TakeDamage(float val, GameObject source = null) => playerCombat.TakeDamage(val, source);
    public void HandleDeath()
    {
        Debug.Log("Handling Character Death!");

        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;
        }

        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        gameObject.layer = LayerMask.NameToLayer("Dead");

        // Character Switching Logic
        var followerManager = GameplayManager.Instance.followerManager;
        int nextIndex = followerManager.GetAvailableFollower();

        if (nextIndex != -1)
        {
            followerManager.SwitchTo(nextIndex);
        }
        else
        {
            // No available characters, trigger game over or team defeat
            GameplayManager.Instance.SetDeadTeam(GameplayManager.Instance.activeTeamID, true);
            GameplayManager.Instance.followerManager.ResetTarget();
            GameplayManager.Instance.gameplayUI.HandleGameOver();
        }
    }
    public bool IsDead() => isDead;
    public string GetCharacterID() => characterData.data.GetID();
    public CharacterBattleState GetCharacterState() => characterData;

    public void AddStatProvider(IStatProvider provider) =>
        characterData.AddStatProvider(provider);

    public void RemoveStatProvider(IStatProvider provider) =>
        characterData.RemoveStatProvider(provider);
    public float GetStat(string statId) => characterData.GetStat(statId);
    public void Fire()
    {
        playerCombat.HandleTrigger();
    }
    // Temporary Change on weapon effects
    public void UpdateFireRate(float rate)
    {
        playerCombat.UpdateFireRate(rate);
    }
    public float GetFirerate() => playerCombat.GetFirerate();
}
