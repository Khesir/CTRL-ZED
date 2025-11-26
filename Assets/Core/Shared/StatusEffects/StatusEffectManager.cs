using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour, IStatusEffectManager
{
    public List<StatusEffectInstance> activeBuffs { get; private set; }
    public event Action onBuffChange;
    private bool isInitialized = false;
    public async UniTask Initialize()
    {
        activeBuffs = new();
        isInitialized = true;
        await UniTask.CompletedTask;
    }
    public void AddBuff(StatusEffectData data)
    {
        StatusEffectInstance instance = new StatusEffectInstance(data);
        activeBuffs.Add(instance);

        onBuffChange?.Invoke();
        Debug.Log($"[StatusEffectManager] Added buff '{data.title}' - will apply when gameplay starts");
    }

    /// <summary>
    /// Applies all stored buffs to existing targets - called when gameplay starts
    /// </summary>
    public void ApplyAllStoredBuffsToScene()
    {
        int totalTargetsAffected = 0;

        foreach (var buffInstance in activeBuffs)
        {
            foreach (var effect in buffInstance.data.effects)
            {
                List<GameObject> targets = FindTargetsByType(effect.Target);
                foreach (var target in targets)
                {
                    buffInstance.ApplyToTarget(target);
                    totalTargetsAffected++;
                }
                Debug.Log($"[StatusEffectManager] Applied {effect.Target} buff '{buffInstance.data.title}' to {targets.Count} targets");
            }
        }

        Debug.Log($"[StatusEffectManager] Applied {activeBuffs.Count} buffs to {totalTargetsAffected} total targets");
    }

    /// <summary>
    /// Applies all active buffs to a newly spawned entity
    /// </summary>
    public void ApplyActiveBuffsToEntity(GameObject entity)
    {
        if (entity == null) return;

        bool hasCharacter = entity.GetComponent<PlayerGameplayService>() != null;
        bool hasEnemy = entity.GetComponent<EnemyService>() != null;

        foreach (var buffInstance in activeBuffs)
        {
            foreach (var effect in buffInstance.data.effects)
            {
                bool shouldApply = false;

                switch (effect.Target)
                {
                    case TargetType.AllPlayers:
                        shouldApply = hasCharacter;
                        break;
                    case TargetType.AllEnemies:
                        shouldApply = hasEnemy;
                        break;
                }

                if (shouldApply)
                {
                    buffInstance.ApplyToTarget(entity);
                    Debug.Log($"[StatusEffectManager] Applied active buff '{buffInstance.data.title}' to new entity {entity.name}");
                }
            }
        }
    }

    /// <summary>
    /// Finds all GameObjects that match the specified target type
    /// </summary>
    private List<GameObject> FindTargetsByType(TargetType targetType)
    {
        List<GameObject> targets = new List<GameObject>();

        switch (targetType)
        {
            case TargetType.AllPlayers:
                // Find all characters with PlayerGameplayService
                var characters = FindObjectsOfType<PlayerGameplayService>();
                targets.AddRange(characters.Select(c => c.gameObject));
                break;

            case TargetType.AllEnemies:
                // Find all enemies with EnemyService
                var enemies = FindObjectsOfType<EnemyService>();
                targets.AddRange(enemies.Select(e => e.gameObject));
                break;

            case TargetType.Player:
            case TargetType.Enemies:
            case TargetType.Follower:
                // These are for targeted effects, not global buffs
                break;
        }

        return targets;
    }

    private void Update()
    {
        if (!isInitialized) return;
        float dt = Time.deltaTime;
        bool buffChanged = false;
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            bool isExpired = activeBuffs[i].Tick(dt);
            if (isExpired)
            {
                activeBuffs.RemoveAt(i);
                buffChanged = true;
            }
        }
        // Only invoke event when buffs actually change to prevent unnecessary calls
        if (buffChanged)
        {
            onBuffChange?.Invoke();
        }
    }
    public void ClearAllBuffs(bool invokeEvent = true)
    {
        foreach (var buff in activeBuffs)
        {
            buff.RemoveFromAll();
        }
        activeBuffs.Clear();

        // Only invoke event if requested (avoid during scene teardown)
        if (invokeEvent)
        {
            onBuffChange?.Invoke();
        }
    }
    public List<StatusEffectInstance> GetAllStatusInstances()
    {
        return activeBuffs;
    }
    public bool IsStatusEffectAdded(StatusEffectData data)
    {
        foreach (var instance in activeBuffs)
        {
            if (instance.data.id == data.id)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsThereExisitingBuffType(StatusEffectData data)
    {
        foreach (var instance in activeBuffs)
        {
            if (instance.data.buffType == data.buffType)
            {
                return true;
            }
        }
        return false;
    }
}
