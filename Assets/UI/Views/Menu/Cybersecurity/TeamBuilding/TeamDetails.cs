using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamDetails : MonoBehaviour
{
    public TeamList teamList;
    public Button setActive;
    public TMP_Text resourceCost;
    public TeamService instance;
    public GameObject emptyCbaracter;
    public void Initialize(TeamService instance)
    {
        this.instance = instance;
        var team = instance.GetMembers();
        SetButton();
        var totalDeploymentCost = new Dictionary<string, float>();
        List<DetailTeamMember> go = new List<DetailTeamMember>
        {
          teamList.member1,
          teamList.member2,
          teamList.member3,
          teamList.member4
        };
        for (int i = 0; i < go.Count; i++)
        {
            if (team[i] != null)
            {
                go[i].SetToState(true); // This changes the go to true / visible
                go[i].Initialize(team[i]);
                var cost = team[i].GetDeploymentCost();

                foreach (var kvp in cost)
                {
                    if (totalDeploymentCost.ContainsKey(kvp.Key))
                    {
                        totalDeploymentCost[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        totalDeploymentCost[kvp.Key] = kvp.Value;
                    }
                }
            }
            else
            {
                go[i].SetToState(false);
            }

        }

        // Format the output string
        string output = "Deployment Cost:\n";
        foreach (var kvp in totalDeploymentCost)
        {
            output += $"{kvp.Key}: {kvp.Value:F2}\n";
        }

        // Display in TMP_Text
        resourceCost.text = output;
        setActive.onClick.RemoveAllListeners();
        setActive.onClick.AddListener(ActionButton);
    }
    public void ActionButton()
    {
        GameManager.Instance.TeamManager.SetActiveTeam(instance.teamID);
        SetButton();
    }
    public void SetButton()
    {
        setActive.interactable = !GameManager.Instance.TeamManager.isTeamActive(instance.teamID);
    }
}
