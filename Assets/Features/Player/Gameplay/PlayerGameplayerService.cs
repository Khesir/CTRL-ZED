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
    [SerializeField] private bool isDead = false;

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
        inputEnabled = enabled;
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

        isDead = true;
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
            GameplayManager.Instance.followerManager.ResetTarget();

            var team = GameManager.Instance.TeamManager.GetActiveTeam();
            GameplayManager.Instance.gameplayUI.Complete(
                type: "character",
                complete: false,
                team[0].GetTeamName()
            );
        }
    }
    public bool IsDead() => isDead;
    public string GetCharacterID() => characterData.data.GetID();
    public CharacterBattleState GetCharacterState() => characterData;
}
