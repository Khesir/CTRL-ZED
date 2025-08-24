using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private Image icon;
    public void Initialize(SkillConfig skillConfig)
    {
        skillName.text = skillConfig.skillName;

        icon.sprite = skillConfig.icon;
    }
}
