using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class P4Antivirus : MonoBehaviour
{
    public Sprite purchaseSprite;
    public TMP_Text title;
    public TMP_Text description;
    public Button purchase;
    public int index;
    public GameObject antiVirusPrefab;
    public Transform content;
    void OnEnable()
    {
        Populate();
    }
    private void Populate()
    {
        var data = GameManager.Instance.AntiVirusManager.antiVirusStage;
        Clear();
        for (int i = 0; i < data.Count; i++)
        {

            var instance = data[i];
            var cardGO = Instantiate(antiVirusPrefab, content);
            var item = cardGO.GetComponent<VirusItem>();

            if (GameManager.Instance.AntiVirusManager.level >= i)
            {
                cardGO.GetComponent<Image>().sprite = purchaseSprite;
                item.price.text = "Purchased";
            }
            else
            {
                item.price.text = data[i].price.ToString();
            }
            item.plan.text = data[i].title;

            int capturedIndex = i;
            item.button.onClick.RemoveAllListeners();
            item.button.onClick.AddListener(() => ActionButton(capturedIndex));
        }
    }
    public void ActionButton(int index)
    {
        var data = GameManager.Instance.AntiVirusManager.antiVirusStage;
        title.text = data[index].title;
        description.text = $"Price:\n{data[index].price} Coins \n\nBuffs:\n -{data[index].buffs.enemySlowRate * 100}% enemy slowrate\n +{data[index].buffs.hpRegenPerSecond} HpRegen per second\n +{data[index].buffs.dex} Dex";
        if (GameManager.Instance.AntiVirusManager.level >= index)
        {
            purchase.gameObject.SetActive(false);
        }
        else
        {
            purchase.gameObject.SetActive(true);
            purchase.onClick.RemoveAllListeners();
            purchase.onClick.AddListener(() => ActionPurchase(data[index].price, index));
        }
    }
    private void ActionPurchase(int price, int index)
    {
        var res = GameManager.Instance.ResourceManager.SpendCoins(price);
        if (res)
        {
            GameManager.Instance.AntiVirusManager.SetLevel(index);
            RefreshUI();
        }
    }
    public void Clear()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
    public void RefreshUI()
    {
        Populate();
    }
}
