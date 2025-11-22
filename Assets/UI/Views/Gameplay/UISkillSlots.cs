using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour
{
    [SerializeField] private List<UISkillSlot> skillSlots;
    [SerializeField] private SkillEventChannel skillEventChannel;
    public void Initialize()
    {
        GameplayManager.Instance.FollowerManager.OnSwitch += Refresh;
        skillEventChannel.OnSkillUsed += OnSkillUsed;
        skillEventChannel.OnSkillsEquipped += Refresh;
    }
    private void Refresh()
    {
        var x = GameplayManager.Instance.FollowerManager.GetCurrentTargetBattleState().data.GetData();
        skillSlots[0].Initialize(x.baseData.skill1);
        skillSlots[1].Initialize(x.baseData.skill2);
    }
    private void OnSkillUsed(int index, float cooldown)
    {
        skillSlots[index].StartCooldown(cooldown);
    }
    private void OnDestroy()
    {
        skillEventChannel.OnSkillUsed -= OnSkillUsed;
        skillEventChannel.OnSkillsEquipped -= Refresh;
    }
}
