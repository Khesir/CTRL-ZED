using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public GameObject ActiveTeamPrefab;
    public GameObject endlessLevel;
    public GameObject LevelPrefab;
    public Transform content;
    public GameObject teamDetails;
    public Transform teamContent;
    public int TeamIndex;
    void OnEnable()
    {
        var levels = GameManager.Instance.LevelManager.levels;
        Clear();
        int currentLevelIndex = GameManager.Instance.LevelManager.currentLevelIndex; // Better naming
        for (int i = 0; i < levels.Count - 1; i++)
        {
            var go = Instantiate(LevelPrefab, content);
            var lvl = go.GetComponent<LevelPrefab>();
            bool isLocked = i > currentLevelIndex;

            lvl.Setup(i, levels[i].icon, $"Level {i + 1}", !isLocked);
        }
        Instantiate(endlessLevel, content);
        var team = Instantiate(ActiveTeamPrefab, teamContent);
        var teamContainer = team.GetComponent<TeamContainer>();
        var activeTeam = GameManager.Instance.TeamManager.GetActiveTeam();
        teamContainer.Setup(activeTeam[0], teamDetails);
    }
    public void Clear()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in teamContent)
        {
            Destroy(child.gameObject);
        }
    }
}
