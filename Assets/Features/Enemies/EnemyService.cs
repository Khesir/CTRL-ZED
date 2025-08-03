using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : MonoBehaviour, IStatHandler
{
    [SerializeField] private EnemyConfig config;

    private float currentHP;
    private EnemyFollow follow;
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
        follow.target = GameplayManager.Instance.followerManager.GetCurrentTarget();
        GameplayManager.Instance.enemyManager.RegisterEnemy(this);
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
            Instantiate(config.destroyEffect, transform.position, Quaternion.identity);

        GameplayManager.Instance.enemyManager.UnregisterEnemy(this);

        if (notifyWaveSystem)
            GameplayManager.Instance.waveManager.ReportKill();

        GameplayManager.Instance.squadLevelManager.GetExperience(config.experienceToGive);

        Destroy(gameObject);
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
