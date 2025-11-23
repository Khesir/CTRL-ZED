using System.Collections.Generic;
using UnityEngine;

public class TeamBuildingController : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject createTeamPrefab;
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private Transform parentLayout;
    [SerializeField] private GameObject TeamDetails;

    private ICharacterManager _characterManager;
    private ITeamManager _teamManager;

    private void OnEnable()
    {
        _characterManager = ServiceLocator.Get<ICharacterManager>();
        _teamManager = ServiceLocator.Get<ITeamManager>();

        Populate();
        _characterManager.onInventoryChange += inventoryUI.RefreshUI;
        _teamManager.onTeamChange += RefreshUI;
    }

    private void OnDisable()
    {
        _characterManager.onInventoryChange -= inventoryUI.RefreshUI;
        _teamManager.onTeamChange -= RefreshUI;
    }

    private void Populate()
    {
        List<TeamService> teamData = _teamManager.GetTeams();
        Clear();
        for (int i = 0; i < teamData.Count; i++)
        {
            TeamService instance = teamData[i];
            var cardGO = Instantiate(teamPrefab, parentLayout);
            var teamContainer = cardGO.GetComponent<TeamContainer>();
            teamContainer.Setup(instance, TeamDetails);
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
