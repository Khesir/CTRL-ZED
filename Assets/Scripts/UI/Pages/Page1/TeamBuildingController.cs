using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBuildingController : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject createTeamPrefab;
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private Transform parentLayout;
    private void OnEnable()
    {
        List<Team> teamData = GameManager.Instance.TeamManager.GetTeams();
        Clear();
        for (int i = 0; i < teamData.Count; i++)
        {
            var instance = teamData[i];
            var cardGO = Instantiate(teamPrefab, parentLayout);
            var teamContainer = cardGO.GetComponent<TeamContainer>();
            teamContainer.Setup(instance, i);
        }
        Instantiate(createTeamPrefab, parentLayout);
        GameManager.Instance.PlayerManager.onInventoryChange += inventoryUI.RefreshUI;
    }
    private void OnDisable()
    {

        GameManager.Instance.PlayerManager.onInventoryChange -= inventoryUI.RefreshUI;
    }
    public void Clear()
    {
        foreach (Transform child in parentLayout)
        {
            Destroy(child.gameObject);
        }
    }
}
