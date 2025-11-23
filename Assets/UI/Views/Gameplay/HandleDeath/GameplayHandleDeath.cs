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

    private IGameplayManager gameplayManager;

    public async void SetDisplay(bool flag = true)
    {
        gameObject.SetActive(flag);
        gameplayManager = ServiceLocator.Get<IGameplayManager>();

        if (flag)
        {
            await panelAnimator.Show();
            gameplayManager.OnDeadTeamUpdated += UpdateDeployedTeam;
            Setup();
        }
        else
        {
            // Disconnect external dependencies
            await panelAnimator.Hide();
            gameplayManager.OnDeadTeamUpdated -= UpdateDeployedTeam;
        }
    }
    private void Setup()
    {
        UpdateDeployedTeam();
        SetupGiveUpButton();
    }


    private void UpdateDeployedTeam()
    {
        List<TeamService> teams = ServiceLocator.Get<ITeamManager>().GetActiveTeam();
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
        driveCounter.text = ServiceLocator.Get<IPlayerManager>()
                        .playerService.GetChargedDrives().ToString();

        giveupButton.onClick.RemoveAllListeners();
        giveupButton.onClick.AddListener(async () =>
        {
            await gameplayManager.SetState(GameplayState.End);
        });
    }

    private async void OnDeploySelected(GamplayTeamCompCard card)
    {
        // Hide the death panel
        SetDisplay(false);

        // Start the round
        await gameplayManager.SetState(GameplayState.Playing);
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
