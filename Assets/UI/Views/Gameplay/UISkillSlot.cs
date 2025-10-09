using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private Image icon;
    [SerializeField] private RadialFillDOTween radial;
    public void Initialize(SkillConfig skillConfig)
    {
        skillName.text = skillConfig.skillName;
        icon.sprite = skillConfig.icon;
        radial.Stop(true);
    }
    public void StartCooldown(float duration)
    {
        radial.Stop();
        radial.SetProgress(1f);
        radial.FillTo(0f, duration);
    }
}
