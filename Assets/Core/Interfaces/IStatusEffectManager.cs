using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IStatusEffectManager
{
    List<StatusEffectInstance> activeBuffs { get; }
    event Action onBuffChange;

    UniTask Initialize();
    void AddBuff(StatusEffectData data);
    void ApplyAllStoredBuffsToScene();
    void ApplyActiveBuffsToEntity(GameObject entity);
    void ClearAllBuffs(bool invokeEvent = true);
    List<StatusEffectInstance> GetAllStatusInstances();
    bool IsStatusEffectAdded(StatusEffectData data);
    bool IsThereExisitingBuffType(StatusEffectData data);
}
