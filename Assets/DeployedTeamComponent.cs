using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeployedTeamComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private List<GameObject> icons;
    [SerializeField] private Sprite nullMember;
    [SerializeField] private GameObject deployButton;
    [SerializeField] private GameObject unDeployButton;
    private TeamService service;

    public void Setup(TeamService service)
    {
        this.service = service;
        IsActive();
        var members = service.GetMembers();
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i] == null)
            {
                icons[i].GetComponent<Image>().sprite = nullMember;
            }
            else
            {
                icons[i].GetComponent<Image>().sprite = members[i].baseData.ship;
            }
        }
        title.text = service.GetTeamName();
        var deployBtn = deployButton.GetComponent<Button>();

        deployBtn.onClick.RemoveAllListeners();
        deployBtn.onClick.AddListener(DeployAction);

        var unDeployBtn = unDeployButton.GetComponent<Button>();
        unDeployBtn.onClick.RemoveAllListeners();
        unDeployBtn.onClick.AddListener(UnDeployAction);
    }
    public void DeployAction()
    {
        Debug.Log($"Deploy {service.teamID}");
        GameManager.Instance.TeamManager.SetActiveTeam(service.teamID);
        IsActive();
        SoundManager.PlaySound(SoundCategory.Team, SoundType.Team_Deploy);
    }
    public void UnDeployAction()
    {
        GameManager.Instance.TeamManager.RemoveActiveTeam(service.teamID);
        Debug.Log($"UnDeploy {service.teamID}");
        IsActive();
        SoundManager.PlaySound(SoundCategory.Team, SoundType.Team_UnDeploy);
    }
    public void IsActive()
    {
        var isActive = GameManager.Instance.TeamManager.isTeamActive(service.teamID);
        if (isActive)
        {
            deployButton.SetActive(false);
            unDeployButton.SetActive(true);
        }
        else
        {
            deployButton.SetActive(true);
            unDeployButton.SetActive(false);
        }
    }
}
