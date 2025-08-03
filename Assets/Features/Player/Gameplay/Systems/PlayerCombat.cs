using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private IInputService input;
    private CharacterBattleState characterData;
    [Header("Player Properties")]
    [SerializeField] private bool isImmune;
    [SerializeField] private float immunityDuration;
    [SerializeField] private float immunityTimer;
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private SkillHolder skillHolder;
    public void Initialize(IInputService input, CharacterBattleState characterData)
    {
        var baseConfig = characterData.data.GetInstance();

        this.input = input;
        this.characterData = characterData;

        weaponHolder = GetComponentInChildren<WeaponHolder>();
        weaponHolder.EquipWeapon(baseConfig.weapon);

        skillHolder = GetComponentInChildren<SkillHolder>();
        skillHolder.skillUser = gameObject;
        skillHolder.EquipSkills(baseConfig.skill1, baseConfig.skill2);
        Debug.Log("[PlayerCombat] Succesfully initialized");
    }

    public void TickUpdate()
    {
        HandleImmunity();
        if (input != null && input.IsFirePressed())
        {
            weaponHolder.Fire();
        }
    }
    public void HandleSkillInput()
    {
        // Currently support 2 skill
        for (int i = 0; i < 2; i++)
        {
            if (input.SkillPressed(i))
            {
                skillHolder.UseSkill(i);
            }
        }
    }
    private void HandleImmunity()
    {
        if (!isImmune) return;

        immunityTimer -= Time.deltaTime;
        if (immunityTimer <= 0f)
        {
            isImmune = false;
        }
    }
    public void TakeDamage(float damage, GameObject source = null)
    {
        if (isImmune) return;
        isImmune = true;
        immunityTimer = immunityDuration;
        bool died = characterData.TakeDamage(damage);
        if (died)
        {
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        gameObject.SetActive(false);

        var follower = GetComponent<Follower>();
        if (follower != null) follower.enabled = false;

        var followerManager = GameplayManager.Instance.followerManager;
        int index = followerManager.GetAvailableFollower();

        if (index != -1)
        {
            followerManager.SwitchTo(index);
        }
        else
        {
            followerManager.ResetTarget();

            var spawner = GameplayManager.Instance.spawner;
            var team = GameManager.Instance.TeamManager.GetActiveTeam();
            var loots = spawner.waves[spawner.waveNumber].waveRewards;

            GameplayManager.Instance.gameplayUI.Complete(
                type: "character",
                complete: false,
                team[0].GetTeamName(),
                loots: loots
            );
        }
    }
}
