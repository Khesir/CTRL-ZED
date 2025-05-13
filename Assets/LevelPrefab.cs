using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class LevelPrefab : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public Button gameplayButton;
    public Image disabled;
    public void Setup(int index, Sprite levelIcon, string title, bool active)
    {
        this.title.text = title;
        icon.sprite = levelIcon;
        gameplayButton.onClick.RemoveAllListeners();
        if (active)
        {
            disabled.gameObject.SetActive(false);
            gameplayButton.interactable = true;
            gameplayButton.onClick.AddListener(() => StartGameplay(index));
        }
        else
        {
            disabled.gameObject.SetActive(true);
            gameplayButton.interactable = false;
        }
    }
    public void StartGameplay(int index)
    {
        var activeTeam = GameManager.Instance.TeamManager.GetActiveTeam();
        if (activeTeam != null)
        {
            if (!GameManager.Instance.ResourceManager.UseRemainingCharge())
            {

                var members = activeTeam.GetMembers();
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
                GameManager.Instance.ResourceManager.SpendFood((int)totalDeploymentCost["Food"]);

                GameManager.Instance.ResourceManager.SpendTechnology((int)totalDeploymentCost["Technology"]);
                GameManager.Instance.ResourceManager.SpendEnergy((int)totalDeploymentCost["Energy"]);
                GameManager.Instance.ResourceManager.SpendIntelligence((int)totalDeploymentCost["Intelligence"]);
            }
            GameManager.Instance.LevelManager.activeLevel = index;
            GameManager.Instance.MainMenu.PlayGame("gameplay");
        }
    }
}
