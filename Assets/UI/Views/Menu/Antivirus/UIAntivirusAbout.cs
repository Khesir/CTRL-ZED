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
    private AntiVirus instance;
    public void Setup(AntiVirus effect)
    {
        instance = effect;
        title.text = effect.effectName;
        price.text = effect.cost.ToString();
        description.text = effect.description;
        icon.sprite = effect.icon;
        purchaseButon.onClick.RemoveAllListeners();
        purchaseButon.onClick.AddListener(ActionButton);
    }
    public void ActionButton()
    {
        var res = GameManager.Instance.PlayerManager.playerService.SpendCoins(instance.cost);
        if (res)
        {
            Debug.Log("Activate Buff");
            // instance.Apply();
        }
    }
}
