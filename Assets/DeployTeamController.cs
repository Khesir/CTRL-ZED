using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployTeamController : MonoBehaviour
{
    [SerializeField] private GameObject NoSelectedTeamMessagePrefab;
    [SerializeField] private GameObject deployedTeamPrefab;
    [SerializeField] private Transform container;
    public void Setup()
    {
        UpdateComponent();
        GameManager.Instance.TeamManager.onTeamChange += UpdateComponent;
    }
    private void UpdateComponent()
    {
        List<TeamService> teams = GameManager.Instance.TeamManager.GetTeams();
        if (teams.Count == 0)
        {
            Instantiate(NoSelectedTeamMessagePrefab, container);
            return;
        }
        Clear();

        foreach (TeamService team in teams)
        {
            var go = Instantiate(deployedTeamPrefab, container);
            var component = go.GetComponent<DeployedTeamComponent>();
            component.Setup(team);
        }
    }
    private void Clear()
    {
        if (container == null) return;

        if (container.childCount > 0)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
