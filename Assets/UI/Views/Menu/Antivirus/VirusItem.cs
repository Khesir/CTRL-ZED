using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VirusItem : MonoBehaviour
{
    public TMP_Text plan;
    public TMP_Text price;
    public Button button;
    public UIAntivirusAbout target;
    private AntiVirus antiVirus;
    public void Setup(AntiVirus effect)
    {
        antiVirus = effect;
        plan.text = effect.effectName;
        price.text = effect.cost.ToString();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ActionButton);
    }
    public void ActionButton()
    {
        target.Setup(antiVirus);
    }

}
