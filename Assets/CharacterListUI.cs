using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListUI : MonoBehaviour
{
    public List<Team> teams;
    public GameObject characterIcon;
    public void Start()
    {
        teams = GameManager.Instance.TeamManager.GetTeams();
        Debug.Log(teams[0].teamName);
    }
    void OnClosed()
    {
        characterIcon.SetActive(true);
    }
}
