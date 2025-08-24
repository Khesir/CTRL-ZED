
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using Vector2 = UnityEngine.Vector2;
using System.Linq;

public class EnemyService : MonoBehaviour, IStatHandler, IDamageable
{
    [SerializeField] private EnemyConfig config;
    [SerializeField] private GameObject lootDropPrefab;
    private float currentHP;
    private EnemyFollow follow;
    public bool isInitialized;

    public bool isDead => currentHP <= 0;
    private readonly List<IStatProvider> statProviders = new();

    public void Initialize(EnemyConfig config)
    {
        this.config = config;
        currentHP = config.maxHealth;

        // Optional visuals
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && config.sprite != null)
        {
            spriteRenderer.sprite = config.sprite;
        }

        follow = gameObject.GetComponent<EnemyFollow>();
        follow.Initialize(this);
        GameplayManager.Instance.enemyManager.RegisterEnemy(this);
        isInitialized = true;
    }
    public void TakeDamage(float damage, GameObject source = null)
    {
        SoundManager.PlaySound(SoundCategory.Gameplay, SoundType.Gameplay_Damage);
        if (damage != -1) GameplayManager.Instance.damageNumberController.CreateNumber(damage, transform.position);

        currentHP -= damage;
        if (isDead)
        {
            Die();
        }
    }
    public void SilentKill()
    {
        Die(notifyWaveSystem: false, isSilent: true);
    }
    private void Die(bool isSilent = false, bool notifyWaveSystem = true)
    {
        if (config.destroyEffect != null)
            Instantiate(config.destroyEffect, transform.position, UnityEngine.Quaternion.identity);

        GameplayManager.Instance.enemyManager.UnregisterEnemy(this);

        if (notifyWaveSystem)
            GameplayManager.Instance.waveManager.ReportKill();

        if (!isSilent) InstantiateLoot(transform.position);

        SoundManager.PlaySound(SoundCategory.Gameplay, SoundType.Gameplay_Explosion);
        Destroy(gameObject);
    }
    private LootDropData GetDropItem()
    {
        List<LootDropData> possibleItems = new List<LootDropData>();
        foreach (var item in config.lootDrops)
        {
            if (Random.value <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            LootDropData droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        return null;
    }
    private void InstantiateLoot(UnityEngine.Vector3 spawnPosition)
    {
        LootDropData droppedItem = GetDropItem();
        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(lootDropPrefab, spawnPosition, UnityEngine.Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.item.icon;

            float dropForce = 300f;
            Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);

            // Registering it to lootmanager
            var collect = lootGameObject.GetComponent<LootCollect>();
            collect.data = droppedItem;
            GameplayManager.Instance.lootManager.RegisterLoot(collect);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerGameplayService>();
        if (player != null && player.isControlled)
        {
            Debug.Log($"Enemy Dealt Damage to player {GetAttack()}");
            player.TakeDamage(GetAttack());
        }
    }
    // Appy Buffs and Debuff
    // -- IStatHanlder
    public void AddStatProvider(IStatProvider provider) => statProviders.Add(provider);
    public void RemoveStatProvider(IStatProvider provider) => statProviders.Remove(provider);
    private float ApplyModifiers(string statId, float basevalue)
    {
        var modifiers = new List<StatModifier>();
        foreach (var provider in statProviders)
        {
            modifiers.AddRange(provider.GetModifiers().Where(m => m.statId == statId));
        }
        modifiers.Sort((a, b) => a.priority.CompareTo(b.priority));

        float flat = 0;
        float percentAdd = 0;
        float percentMult = 1f;

        foreach (var mod in modifiers)
        {
            switch (mod.type)
            {
                case ModifierType.Flat: flat += mod.value; break;
                case ModifierType.PercentAdd: percentAdd += mod.value; break;
                case ModifierType.PercentMult: percentMult *= 1 + mod.value; break;
            }
        }
        return (basevalue + flat) * (1 + percentAdd) * percentMult;
    }
    // Derived Stats
    public int GetAttack() => Mathf.RoundToInt(ApplyModifiers("ATK", config.baseDamage + (config.difficultyMultiplier * 2)));
    public int GetDefense() => Mathf.RoundToInt(ApplyModifiers("DEF", config.defense + (config.difficultyMultiplier * 1.5f)));
    public int GetDexterity() => Mathf.RoundToInt(ApplyModifiers("DEX", config.dex));
    public int GetMaxHealth() => Mathf.RoundToInt(ApplyModifiers("HP", config.maxHealth + (config.difficultyMultiplier * 10)));
    public int GetSpeed() => Mathf.RoundToInt(ApplyModifiers("SPD", config.dex));
    public Dictionary<string, int> GetStatMap()
    {
        return new()
        {
            { "ATK", GetAttack() },
            { "DEF", GetDefense() },
            { "DEX", GetDexterity() },
            { "HP", GetMaxHealth() },
            { "SPD", GetSpeed() }
        };
    }
    public float GetStat(string id)
    {
        switch (id)
        {
            case "ATK":
                return GetAttack();
            case "DEF":
                return GetDefense();
            case "DEX":
                return GetDexterity();
            case "HP":
                return GetMaxHealth();
        }
        return -1;
    }
    public EnemyConfig GetConfig() => config;
}
