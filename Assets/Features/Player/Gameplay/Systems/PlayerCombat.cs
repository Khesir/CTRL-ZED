using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private IInputService input;
    private ISoundService soundService;
    private CharacterBattleState characterData;
    private CommandQueue commandQueue;

    [Header("Player Properties")]
    [SerializeField] private bool isImmune;
    [SerializeField] private float immunityDuration;
    [SerializeField] private float immunityTimer;
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private SkillHolder skillHolder;
    [SerializeField] private ShaderDamageFlash damageFlash;

    public void Initialize(IInputService input, CharacterBattleState characterData)
    {
        this.input = input;
        this.characterData = characterData;
        soundService = ServiceLocator.Get<ISoundService>();
        commandQueue = new CommandQueue();

        var baseConfig = characterData.data.GetInstance();
        immunityDuration = 2f;

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
            HandleTrigger();
        }

        // Process any queued commands
        commandQueue.ProcessAll();
    }

    public void HandleTrigger()
    {
        // Use command pattern for firing
        commandQueue.ExecuteImmediate(new FireCommand(weaponHolder));
    }

    public void HandleSkillInput()
    {
        // Use command pattern for skills
        for (int i = 0; i < 2; i++)
        {
            if (input.SkillPressed(i))
            {
                commandQueue.ExecuteImmediate(new UseSkillCommand(skillHolder, i));
            }
        }
    }
    private void HandleImmunity()
    {
        if (!isImmune) return;

        soundService.PlayForDuration(SoundCategory.Status, SoundType.Status_Immune, immunityDuration);

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
        soundService.Play(SoundCategory.Gameplay, SoundType.Gameplay_Damage);
    }

    // Temporary Change on weapon effects
    public void UpdateFireRate(float rate)
    {
        weaponHolder.UpdateFirerate(rate);
    }
    public float GetFirerate() => weaponHolder.GetFirerate();
}
