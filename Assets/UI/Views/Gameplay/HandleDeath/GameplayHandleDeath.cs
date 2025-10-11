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
    [SerializeField] private PanelAnimator panelAnimator;
    public async void SetDisplay(bool flag = true)
    {
        gameObject.SetActive(flag);
        if (flag)
        {
            await panelAnimator.Show();
            GameplayManager.Instance.onUpdateDeadTeam += UpdateDeployedTeam;
            Setup();
        }
        else
        {
            // Disconnect external dependencies
            await panelAnimator.Hide();
            GameplayManager.Instance.onUpdateDeadTeam -= UpdateDeployedTeam;
        }
    }
    private void Setup()
    {
        UpdateDeployedTeam();
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
            component.onDeploySelected += OnDeploySelected;
        }
    }
    private async void SetupGiveUpButton()
    {
        driveCounter.text = GameManager.Instance.PlayerManager
                        .playerService.GetChargedDrives().ToString();

        giveupButton.onClick.RemoveAllListeners();
        giveupButton.onClick.AddListener(async () =>
        {
            await GameplayManager.Instance.SetState(GameplayManager.GameplayState.End);
        });
    }
    private async void OnDeploySelected(GamplayTeamCompCard card)
    {
        // Hide the death panel
        SetDisplay(false);

        // Start the round
        await GameplayManager.Instance.SetState(GameplayManager.GameplayState.Playing);
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
