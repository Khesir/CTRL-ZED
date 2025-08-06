
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using Vector2 = UnityEngine.Vector2;

public class EnemyService : MonoBehaviour, IStatHandler
{
    [SerializeField] private EnemyConfig config;
    [SerializeField] private GameObject lootDropPrefab;
    private float currentHP;
    private EnemyFollow follow;
    public bool isInitialized;
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
        follow.Initialize(config);
        follow.target = GameplayManager.Instance.followerManager.GetCurrentTarget();
        GameplayManager.Instance.enemyManager.RegisterEnemy(this);
        isInitialized = true;
    }
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }
    public void SilentKill()
    {
        Die(notifyWaveSystem: false);
    }
    private void Die(bool notifyWaveSystem = true)
    {
        if (config.destroyEffect != null)
            Instantiate(config.destroyEffect, transform.position, UnityEngine.Quaternion.identity);

        GameplayManager.Instance.enemyManager.UnregisterEnemy(this);

        if (notifyWaveSystem)
            GameplayManager.Instance.waveManager.ReportKill();

        GameplayManager.Instance.squadLevelManager.GetExperience(config.experienceToGive);
        InstantiateLoot(transform.position);
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
        if (player != null)
        {
            player.TakeDamage(config.damage);
            // Die();
        }
    }
    // Appy Buffs and Debuff
    public void AddStatProvider(IStatProvider provider)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveStatProvider(IStatProvider provider)
    {
        throw new System.NotImplementedException();
    }
}
