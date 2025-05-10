using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBuildingController : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject createTeamPrefab;
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private Transform parentLayout;
    [SerializeField] private GameObject TeamDetails;
    private void OnEnable()
    {
        Populate();
        GameManager.Instance.CharacterManager.onInventoryChange += inventoryUI.RefreshUI;
        GameManager.Instance.TeamManager.onTeamChange += RefreshUI;
    }
    private void OnDisable()
    {
        GameManager.Instance.CharacterManager.onInventoryChange -= inventoryUI.RefreshUI;
        GameManager.Instance.TeamManager.onTeamChange -= RefreshUI;
    }
    private void Populate()
    {
        List<TeamService> teamData = GameManager.Instance.TeamManager.GetTeams();

        Clear();
        for (int i = 0; i < teamData.Count; i++)
        {
            var instance = teamData[i];
            var cardGO = Instantiate(teamPrefab, parentLayout);
            var teamContainer = cardGO.GetComponent<TeamContainer>();
            teamContainer.Setup(instance, i, TeamDetails);
        }
        var go = Instantiate(createTeamPrefab, parentLayout);
        go.GetComponent<CreateTeam>().Initialize();
    }

    public void Clear()
    {
        foreach (Transform child in parentLayout)
        {
            Destroy(child.gameObject);
        }
    }
    public void RefreshUI()
    {
        Populate();
    }
}
