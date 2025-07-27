using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameplayUIController gameplayUI;

    private FollowerService service = new();

    public void Initialize(List<Follower> followers)
    {
        service.Initialize(virtualCamera, gameplayUI);
        service.SetFollowers(followers);
        service.SwitchTo(0);
    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                service.SwitchTo(i);
            }
        }
    }

    public void AddFollower(Follower follower)
    {
        service.AddFollower(follower);
    }

    public Transform GetCurrentTarget() => service.GetCurrentTarget();
    public void ResetTarget() => service.ResetTarget();
    public int GetAvailableFollower() => service.GetAvailableFollower();
    public void SwitchTo(int newIndex) => service.SwitchTo(newIndex);
}
