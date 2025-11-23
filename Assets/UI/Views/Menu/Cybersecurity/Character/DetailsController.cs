using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsController : MonoBehaviour
{
    [Header("References")]
    public Transform content;
    public GameObject prefab;
    public TMP_Text className;
    public TMP_Text characterName;
    public Image characterIcon;
    public Image characterShip;
    public Detailskills detailskills;
    public Button upgradeButton;
    public TMP_Text buttonText;
    public CharacterData character;
    public int nextLevelCost = 0;
    public void Intialize(CharacterData data)
    {
        Debug.Log(data);
        character = data;

        Populate();
        ServiceLocator.Get<IPlayerManager>().playerService.OnSpendDrives += Populate;
    }
    public void Populate()
    {
        characterName.text = character.name;
        className.text = $"{character.baseData.className} - Lvl {character.currentLevel}";
        characterIcon.sprite = character.baseData.icon;
        characterShip.sprite = character.baseData.ship;
        Clear();
        float multiplier = Mathf.Pow(1.2f, character.currentLevel - 1);

        var statsMap = new Dictionary<string, float>{
            {"Food", character.baseData.food * multiplier },
            {"Technology", character.baseData.technology * multiplier},
            {"Energy", character.baseData.energy * multiplier},
            {"Intelligence", character.baseData.intelligence* multiplier}
        };
        foreach (var instance in statsMap)
        {
            var statCard = Instantiate(prefab, content);
            var card = statCard.GetComponent<StatSlotUI>();
            card.Setup(instance);
        }
        detailskills.Initialize(
            new List<SkillConfig>
            {
                character.baseData.skill1,
                character.baseData.skill2
            }
        );
        buttonText.text = LevelingSystem.CharacterCurve.GetCostCurve(character.currentLevel + 1).ToString();
        nextLevelCost = LevelingSystem.CharacterCurve.GetCostCurve(character.currentLevel + 1);
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(Upgrade);
        // Disabled incase it doesnt require the given requirement
        if (ServiceLocator.Get<IPlayerManager>().playerService.GetChargedDrives() >= nextLevelCost)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }

    public void Clear()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
    private void Upgrade()
    {
        if (nextLevelCost > 0)
        {
            ServiceLocator.Get<IPlayerManager>().playerService.SpendChargeDrives(nextLevelCost);
            character.currentLevel++;
            ServiceLocator.Get<ISoundService>().Play(SoundCategory.Coins, SoundType.Coins_spend);
        }
    }
}
