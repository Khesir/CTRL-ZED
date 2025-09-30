using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHandleDeath : MonoBehaviour
{
    [SerializeField] private GameObject card;
    [SerializeField] private Transform container;
    [SerializeField] private Button giveupButton;
    [SerializeField] private TMP_Text driveCounter;
    private GameplayManager gameplayManager;
    public void SetDisplay(bool flag = true)
    {
        this.gameObject.SetActive(flag);
        if (flag)
        {
            GameplayManager.Instance.onUpdateDeadTeam += UpdateDeployedTeam;
            Setup();
        }
        else
        {
            GameplayManager.Instance.onUpdateDeadTeam -= UpdateDeployedTeam;
            // Disconnect external dependencies
        }
    }
    private void Setup()
    {
        gameplayManager = GameplayManager.Instance;
        UpdateDeployedTeam();
        // Give up Button
        SetupGiveUpButton();
    }


    private void UpdateDeployedTeam()
    {
        List<TeamService> teams = GameManager.Instance.TeamManager.GetActiveTeam();
        if (teams.Count == 0)
        {
            Debug.LogWarning("No set Deployed team");
            return;
        }
        Clear();
        foreach (TeamService team in teams)
        {
            var go = Instantiate(card, container);
            var component = go.GetComponent<GamplayTeamCompCard>();
            component.Setup(team);
        }
    }
    private void SetupGiveUpButton()
    {
        driveCounter.text = GameManager.Instance.PlayerManager
                        .playerService.GetChargedDrives().ToString();

        giveupButton.onClick.RemoveAllListeners();
        giveupButton.onClick.AddListener(() =>
        {
            // Close Game,
        });
    }
    private void Clear()
    {
        if (container.childCount > 0)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
