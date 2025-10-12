using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UIAntivirusAbout : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text price;
    public TMP_Text description;
    public Button purchaseButon;
    private StatusEffectData instance;
    public void Setup(StatusEffectData effect)
    {
        instance = effect;
        title.text = effect.title;
        price.text = $"{effect.price} drives";
        description.text = effect.description;
        icon.sprite = effect.icon;
        var exists = GameManager.Instance.StatusEffectManager.IsThereExisitingBuffType(instance);

        if (exists)
        {
            purchaseButon.interactable = false;
        }
        else
        {
            purchaseButon.interactable = true;
            purchaseButon.onClick.RemoveAllListeners();
            purchaseButon.onClick.AddListener(ActionButton);
        }


    }
    public void ActionButton()
    {
        var res = GameManager.Instance.PlayerManager.playerService.SpendChargeDrives(instance.price);
        if (res)
        {
            SoundManager.PlaySound(SoundCategory.Coins, SoundType.Coins_spend);
            GameManager.Instance.StatusEffectManager.AddBuff(instance);
        }
        else
        {
            SoundManager.PlaySound(SoundCategory.UI, SoundType.UI_Error);
        }
    }
}
