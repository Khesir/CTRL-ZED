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
    public int index;
    [Header("Icons")]
    public GameObject normalIcon;
    public GameObject activeAction;

    private GameObject teamDetails;
    public void Setup(TeamService team, int index, GameObject teamDetails)
    {
        if (team.isActive)
        {
            activeAction.SetActive(true);
        }
        else
        {
            activeAction.SetActive(false);
        }
        this.teamDetails = teamDetails;
        instance = team;
        teamName.text = team.GetTeamName();
        var members = team.GetMembers();
        for (int i = 0; i < team.GetMembers().Count; i++)
        {
            var slot = members[i];
            this.index = index;
            var cardGO = Instantiate(teamSlotPrefab, content);
            var slotData = cardGO.GetComponent<TeamInventorySlot>();
            slotData.teamService = instance;
            slotData.teamId = index;
            slotData.slotIndex = i;
            teamSlot.Add(cardGO);

            if (slot != null)
            {
                var itemGO = Instantiate(teamDraggrable, cardGO.transform);
                var draggable = itemGO.GetComponent<DraggableItem>();
                draggable.Setup(slot);
            }
        }
        viewTeam.onClick.RemoveAllListeners();
        viewTeam.onClick.AddListener(ShowTeam);
    }
    public void ShowTeam()
    {
        teamDetails.SetActive(true);
        teamDetails.GetComponent<TeamDetails>().Initialize(instance, index);
    }
}
