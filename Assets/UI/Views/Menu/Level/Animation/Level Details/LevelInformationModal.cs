using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInformationModal : MonoBehaviour
{
    public Animator animator;
    public Image image;
    public TMP_Text title;
    public TMP_Text objective;
    public TMP_Text description;
    public TMP_Text recommendation;
    public Button button;
    [HideInInspector] public LevelData data;
    public void Trigger()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Close");
    }

    public void CloseTrigger()
    {
        animator.SetTrigger("Close");

        StartCoroutine(WaitUntilReadyThenDisable());
    }
    private IEnumerator WaitUntilReadyThenDisable()
    {
        while (animator.IsInTransition(0))
            yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Ready"))
        {
            yield return null; // Wait 1 frame
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        Debug.Log(data.levelName);
        image.sprite = data.levelIcon;
        title.text = data.levelName;
        objective.text = data.objective;
        recommendation.text = data.recommendation;
        description.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StartGame);
    }
    void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
    void StartGame()
    {
        var activeTeam = GameManager.Instance.TeamManager.GetActiveTeam();

        PlayerService playerService = GameManager.Instance.PlayerManager.playerService;
        // IBioChipService bioChipService = playerService;
        IResourceService resourceService = playerService;
        if (activeTeam.Count < 1)
        {
            Debug.LogWarning("No set active team");
            return;
        }
        var members = activeTeam[0].GetMembers();
        var totalDeploymentCost = new Dictionary<string, float>();

        foreach (CharacterService character in members)
        {
            var cost = character.GetDeploymentCost();
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
        resourceService.SpendFood((int)totalDeploymentCost["Food"]);
        resourceService.SpendTechnology((int)totalDeploymentCost["Technology"]);
        resourceService.SpendEnergy((int)totalDeploymentCost["Energy"]);
        resourceService.SpendIntelligence((int)totalDeploymentCost["Intelligence"]);

        GameManager.Instance.LevelManager.activeLevel = data;
        GameManager.Instance.LevelManager.LoadScene(GameState.Gameplay);
    }
}
