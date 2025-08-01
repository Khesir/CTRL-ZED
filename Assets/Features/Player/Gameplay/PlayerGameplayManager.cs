using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayManager : MonoBehaviour
{
    private GameObject playerGO;
    [SerializeField] private List<PlayerGameplayService> playerService;
    [SerializeField] private int activePlayerIndex = 0;

    public void Initialize(List<GameObject> playerGOs, IInputService inputService)
    {
        // We can possibly scale this to handle multiple team
        // CUrrently hanlde just 1 team
        if (playerGOs == null || playerGOs.Count == 0) return;

        playerService = new List<PlayerGameplayService>();
        for (int i = 0; i < playerGOs.Count; i++)
        {
            var service = playerGOs[i].GetComponent<PlayerGameplayService>();
            service.SetInputService(inputService);
            service.Initialize();
            playerService.Add(service);
        }
        Debug.Log($"[PlayerGameplayManager] Successfully Initialized with {playerGOs.Count + 1} objects");
    }

    public void SetActivePlayer(int index)
    {
        activePlayerIndex = index;

        for (int i = 0; i < playerService.Count; i++)
        {
            playerService[i].SetInputEnabled(i == activePlayerIndex);
        }

        playerGO = playerService[activePlayerIndex].gameObject;
    }

    public Vector3 GetPlayerPosition() => playerGO.transform.position;
}
