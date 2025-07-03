using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamContainer : MonoBehaviour
{
    public Transform content;
    public TMP_Text teamName;
    public GameObject teamSlotPrefab;
    public GameObject teamDraggrable;
    public List<GameObject> teamSlot;
    public TeamService instance;
    public Button viewTeam;
    [Header("Icons")]
    public GameObject normalIcon;
    public GameObject activeAction;
    public GameObject deployAction;
    private GameObject teamDetails;
    public void Setup(TeamService team, GameObject teamDetails)
    {
        instance = team;
        IsActive();
        this.teamDetails = teamDetails;
        teamName.text = team.GetTeamName();
        var members = team.GetMembers();
        for (int i = 0; i < team.GetMembers().Count; i++)
        {
            var slot = members[i];
            var cardGO = Instantiate(teamSlotPrefab, content);
            var slotData = cardGO.GetComponent<TeamInventorySlot>();
            slotData.teamService = instance;
            slotData.slotIndex = i;
            teamSlot.Add(cardGO);

            if (slot != null)
            {
                var itemGO = Instantiate(teamDraggrable, cardGO.transform);
                var draggable = itemGO.GetComponent<DraggableItem>();
                draggable.Setup(slot);
            }
        }
        if (viewTeam != null)
        {
            viewTeam.onClick.RemoveAllListeners();
            viewTeam.onClick.AddListener(ShowTeam);
            Debug.Log("Added Team Details view");
        }

        // Attach actions
        var deployBtn = deployAction.GetComponent<Button>();

        deployBtn.onClick.RemoveAllListeners();
        deployBtn.onClick.AddListener(DeployAction);

        var activeBtn = activeAction.GetComponent<Button>();
        activeBtn.onClick.RemoveAllListeners();
        activeBtn.onClick.AddListener(UnDeployAction);
    }
    public void ShowTeam()
    {
        teamDetails.SetActive(true);
        teamDetails.GetComponent<TeamDetails>().Initialize(instance);
    }
    public void DeployAction()
    {
        Debug.Log($"Deploy {instance.teamID}");
        GameManager.Instance.TeamManager.SetActiveTeam(instance.teamID);
        IsActive();
    }
    public void UnDeployAction()
    {
        GameManager.Instance.TeamManager.RemoveActiveTeam(instance.teamID);
        Debug.Log($"UnDeploy {instance.teamID}");
        IsActive();
    }
    public void IsActive()
    {
        var isActive = GameManager.Instance.TeamManager.isTeamActive(instance.teamID);
        if (isActive)
        {
            activeAction.SetActive(true);
            deployAction.SetActive(false);
        }
        else
        {
            deployAction.SetActive(true);
            activeAction.SetActive(false);
        }
    }
}
