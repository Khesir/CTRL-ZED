using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour
{
    [SerializeField] private List<UISkillSlot> skillSlots;
    public void Initialize()
    {
        GameplayManager.Instance.followerManager.OnSwitch += Refresh;
    }
    private void Refresh()
    {
        var x = GameplayManager.Instance.followerManager.GetCurrentTargetBattleState().data.GetData();
        skillSlots[0].Initialize(x.baseData.skill1);
        skillSlots[1].Initialize(x.baseData.skill2);
    }
}
