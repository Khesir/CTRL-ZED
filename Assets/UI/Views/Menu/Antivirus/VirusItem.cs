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
    private StatusEffectData antiVirus;
    public void Setup(StatusEffectData effect)
    {
        antiVirus = effect;
        plan.text = effect.title;
        price.text = $"{effect.price} drives";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ActionButton);
    }
    public void ActionButton()
    {
        ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_Button);
        target.Setup(antiVirus);
    }

}
