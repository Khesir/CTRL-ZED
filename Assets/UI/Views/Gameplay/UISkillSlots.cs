using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour
{
    [SerializeField] private List<UISkillSlot> skillSlots;
    [SerializeField] private SkillEventChannel skillEventChannel;
    private IFollowerManager followerManager;

    public void Initialize()
    {
        followerManager = ServiceLocator.Get<IFollowerManager>();
        followerManager.OnSwitch += Refresh;
        skillEventChannel.OnSkillUsed += OnSkillUsed;
        skillEventChannel.OnSkillsEquipped += Refresh;
    }

    private void Refresh()
    {
        var x = followerManager.GetCurrentTargetBattleState().data.GetData();
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
