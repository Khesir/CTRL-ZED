using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamplayTeamCompCard : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text teamName;
    [SerializeField] private List<Image> icons;
    [SerializeField] private Button reviveButton;
    [SerializeField] private Button deployButton;

    private TMP_Text deployButtonText;
    private TeamService service;
    private Color defaultBackgroundColor;
    private Color deadColor;

    public Action<GamplayTeamCompCard> onDeploySelected;

    private void Awake()
    {
        deployButtonText = deployButton.GetComponentInChildren<TMP_Text>();
        defaultBackgroundColor = backgroundImage.color;

        // Prepare dead color (#FF4C4C)
        ColorUtility.TryParseHtmlString("#FF4C4C", out deadColor);
    }
    private void OnEnable()
    {
        if (GameplayManager.Instance != null)
            GameplayManager.Instance.OnDeadTeamUpdated += Refresh;
    }

    private void OnDisable()
    {
        if (GameplayManager.Instance != null)
            GameplayManager.Instance.OnDeadTeamUpdated -= Refresh;
    }
    public void Setup(TeamService service)
    {
        this.service = service;
        Refresh(); // Apply initial UI state

        var characters = service.GetMembers();
        for (int i = 0; i < icons.Count && i < characters.Count; i++)
            icons[i].sprite = characters[i].baseData.ship;

        reviveButton.onClick.RemoveAllListeners();
        reviveButton.onClick.AddListener(ReviveAction);

        deployButton.onClick.RemoveAllListeners();
        deployButton.onClick.AddListener(DeployAction);
    }
    private void Refresh()
    {
        if (service == null) return;

        bool isDead = GameplayManager.Instance.IsTeamDead(service.GetData().teamID);
        bool isDeployed = service.GetData().teamID == GameplayManager.Instance.ActiveTeamID;

        // --- Background color ---
        backgroundImage.color = isDead ? deadColor : defaultBackgroundColor;

        // --- Revive button ---
        reviveButton.interactable = isDead;

        // --- Deploy button ---
        if (isDeployed)
        {
            deployButtonText.text = "Deployed";
            deployButton.interactable = false;
        }
        else
        {
            deployButtonText.text = "Deploy";
            deployButton.interactable = !isDead;
        }
    }
    private void DeployAction()
    {
        // End current battle and switch team
        GameplayManager.Instance.EnemyManager.KillAllEnemies(true);
        GameplayManager.Instance.ActiveTeamID = service.GetData().teamID;

        onDeploySelected?.Invoke(this);
    }

    private void ReviveAction()
    {
        GameManager.Instance.PlayerManager.playerService.SpendChargeDrives(1);

        GameplayManager.Instance.SetDeadTeam(service.GetData().teamID, false);
    }
}
