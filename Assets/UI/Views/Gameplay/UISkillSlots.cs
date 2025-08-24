using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour
{
    [SerializeField] private List<UISkillSlot> skillSlots;
    public void Initialize()
    {
        Refersh();
        GameplayManager.Instance.followerManager.OnSwitch += Refersh;
    }
    private void Refersh()
    {
        var x = GameplayManager.Instance.followerManager.GetCurrentTargetBattleState().data.GetData();
        skillSlots[0].Initialize(x.baseData.skill1);
        skillSlots[1].Initialize(x.baseData.skill2);
    }
}
