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
    private ISoundService soundService;

    public void Setup(TeamService service)
    {
        this.service = service;
        soundService = ServiceLocator.Get<ISoundService>();

        UpdateIcons();
        UpdateActiveState();

        title.text = service.GetTeamName();

        deployButton.GetComponent<Button>().onClick.RemoveAllListeners();
        deployButton.GetComponent<Button>().onClick.AddListener(DeployAction);

        unDeployButton.GetComponent<Button>().onClick.RemoveAllListeners();
        unDeployButton.GetComponent<Button>().onClick.AddListener(UnDeployAction);
    }

    private void UpdateIcons()
    {
        var members = service.GetMembers();
        for (int i = 0; i < members.Count; i++)
        {
            var sprite = members[i]?.baseData.ship ?? nullMember;
            icons[i].GetComponent<Image>().sprite = sprite;
        }
    }

    private void DeployAction()
    {
        ServiceLocator.Get<ITeamManager>().SetActiveTeam(service.GetData().teamID);
        UpdateActiveState();
        soundService.Play(SoundCategory.Team, SoundType.Team_Deploy);
    }

    private void UnDeployAction()
    {
        ServiceLocator.Get<ITeamManager>().RemoveActiveTeam(service.GetData().teamID);
        UpdateActiveState();
        soundService.Play(SoundCategory.Team, SoundType.Team_UnDeploy);
    }
    private void UpdateActiveState()
    {
        var isActive = ServiceLocator.Get<ITeamManager>().isTeamActive(service.GetData().teamID);
        deployButton.SetActive(!isActive);
        unDeployButton.SetActive(isActive);
    }
}
