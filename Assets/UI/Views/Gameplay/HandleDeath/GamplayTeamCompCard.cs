using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamplayTeamCompCard : MonoBehaviour
{
    private TeamService service;
    public void Setup(TeamService service)
    {
        this.service = service;
    }
}
