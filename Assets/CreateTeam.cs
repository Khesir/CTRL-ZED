using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTeam : MonoBehaviour
{
    public void CreateTeamGroup()
    {
        GameManager.Instance.TeamManager.CreateTeam();
    }
}
