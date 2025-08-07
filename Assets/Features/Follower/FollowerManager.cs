using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FollowerManager : MonoBehaviour
{
    public CinemachineVirtualCamera focusCamera;
    public GameplayUIController gameplayUI;
    private Transform globalTargetPlayer;
    [SerializeField] private FollowerSpawn followerSpawn;

    [SerializeField] private FollowerService service = new();
    public event Action OnSwitch;
    public List<GameObject> Initialize(List<CharacterBattleState> data)
    {
        List<Follower> followers = followerSpawn.SpawnedFollowers(data);
        List<GameObject> followerObjects = followers.Select(f => f.gameObject).ToList();

        service.Initialize(followers);
        // Subscribe event
        Debug.Log("[FollowerManager] Successfully Initilized all followers");
        return followerObjects;
    }

    private void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchTo(i);
            }
        }
    }
    public void SwitchTo(int index)
    {
        service.SwitchTo(index);
        var newFollower = service.GetCurrentFollower();
        focusCamera.Follow = newFollower.transform;
        globalTargetPlayer = newFollower.transform;
        OnSwitch?.Invoke();
        Debug.Log($"[FollowerManager] Switching to {newFollower.GetCharacterID()}");
    }
    public void AddFollower(Follower follower)
    {
        service.AddFollower(follower);
    }
    public void ResetTarget() => service.ResetTarget();
    public int GetAvailableFollower() => service.GetAvailableFollower();
    public Transform GetCurrentTarget()
    {
        return globalTargetPlayer;
    }
    public CharacterBattleState GetCurrentTargetBattleState()
    {
        return service.GetCurrentFollower().GetCharacterState();
    }
}
