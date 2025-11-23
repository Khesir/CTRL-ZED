using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IStatusEffectManager
{
    List<StatusEffectInstance> activeBuffs { get; }
    event Action onBuffChange;

    UniTask Initialize();
    void AddBuff(StatusEffectData data);
    void ClearAllBuffs();
    List<StatusEffectInstance> GetAllStatusInstances();
    bool IsStatusEffectAdded(StatusEffectData data);
    bool IsThereExisitingBuffType(StatusEffectData data);
}
