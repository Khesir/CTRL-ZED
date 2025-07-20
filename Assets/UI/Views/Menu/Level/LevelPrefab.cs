using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class LevelPrefab : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text objective;
    public Image disabled;
    public void Setup(LevelData data)
    {
        var button = GetComponent<Button>();
        title.text = data.levelName;
        icon.sprite = data.levelIcon;
        objective.text = data.objective;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Active);

        // gameplayButton.onClick.RemoveAllListeners();
        // if (active)
        // {
        //     disabled.gameObject.SetActive(false);
        //     gameplayButton.interactable = true;
        //     gameplayButton.onClick.AddListener(() => StartGameplay(index));
        // }
        // else
        // {
        //     disabled.gameObject.SetActive(true);
        //     gameplayButton.interactable = false;
        // }
    }
    public void Active()
    {
        Debug.Log("Test");
    }
    public void StartGameplay(int index)
    {

        var activeTeam = GameManager.Instance.TeamManager.GetActiveTeam();

        PlayerService playerService = GameManager.Instance.PlayerManager.playerService;
        // IBioChipService bioChipService = playerService;
        IResourceService resourceService = playerService;
        if (activeTeam != null)
        {
            // Set DEFAULT 1
            // if (!bioChipService.SpendRemainingCharge(1))
            if (true)
            {

                var members = activeTeam[0].GetMembers();
                var totalDeploymentCost = new Dictionary<string, float>();

                foreach (CharacterService character in members)
                {
                    var cost = character.GetDeploymentCost();
                    foreach (var kvp in cost)
                    {
                        if (totalDeploymentCost.ContainsKey(kvp.Key))
                        {
                            totalDeploymentCost[kvp.Key] += kvp.Value;
                        }
                        else
                        {
                            totalDeploymentCost[kvp.Key] = kvp.Value;
                        }
                    }
                }
                resourceService.SpendFood((int)totalDeploymentCost["Food"]);
                resourceService.SpendTechnology((int)totalDeploymentCost["Technology"]);
                resourceService.SpendEnergy((int)totalDeploymentCost["Energy"]);
                resourceService.SpendIntelligence((int)totalDeploymentCost["Intelligence"]);
            }
            // GameManager.Instance.LevelManager.activeLevel = ;
            // GameManager.Instance.MainMenu.PlayGame("gameplay");
        }
    }
}
