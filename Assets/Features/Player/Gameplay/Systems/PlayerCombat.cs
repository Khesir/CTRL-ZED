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
    [SerializeField] private ShaderDamageFlash damageFlash;
    public void Initialize(IInputService input, CharacterBattleState characterData)
    {
        var baseConfig = characterData.data.GetInstance();
        immunityDuration = 2f;
        this.input = input;
        this.characterData = characterData;

        weaponHolder = GetComponentInChildren<WeaponHolder>();
        weaponHolder.EquipWeapon(baseConfig.weapon, gameObject, SourceType.Player);

        skillHolder = GetComponentInChildren<SkillHolder>();
        skillHolder.skillUser = gameObject;
        skillHolder.EquipSkills(baseConfig.skill1, baseConfig.skill2);

        damageFlash = GetComponent<ShaderDamageFlash>();
        damageFlash.Initialize();

        // Register relative events
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
        _ = SoundManager.PlayLoopUntil(SoundCategory.Gameplay, SoundType.Gameplay_Immune, immunityDuration);

        immunityTimer -= Time.deltaTime;
        if (immunityTimer <= 0f)
        {
            isImmune = false;
        }
    }
    public void TakeDamage(float damage, GameObject source = null)
    {
        if (isImmune) return;
        damageFlash.Flash(immunityDuration);
        isImmune = true;
        immunityTimer = immunityDuration;
        characterData.TakeDamage(damage);
        SoundManager.PlaySound(SoundCategory.Gameplay, SoundType.Gameplay_Damage);
        _ = SoundManager.PlayLoopUntil(SoundCategory.Gameplay, SoundType.Gameplay_Immune, immunityDuration);
    }
}
