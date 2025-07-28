using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayManager : MonoBehaviour
{
    private GameObject playerGO;
    [SerializeField] private List<PlayerGameplayService> playerService;

    public void Initialize(List<GameObject> playerGOs)
    {
        foreach (var player in playerGOs)
        {
            var playerService = player.GetComponent<PlayerGameplayService>();
            playerService.Initialize();
        }
    }

    public void DisablePlayerControls(int index)
    {
        for (int i = 0; i < playerService.Count; i++)
        {
            if (index == i) playerService[0].enabled = false;
        }
    }
    public void EnablePlayerControls(int index)
    {

        for (int i = 0; i < playerService.Count; i++)
        {
            if (index == i) playerService[0].enabled = true;
        }
    }
    public Vector3 GetPlayerPosition() => playerGO.transform.position;
}
