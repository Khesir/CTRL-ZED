using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowerService
{
    private readonly List<Follower> followers = new();
    private int currentFollowerIndex = 0;
    private CinemachineVirtualCamera camera;
    private Transform globalTargetPlayer;
    private GameplayUIController ui;
    public event Action OnFollowerSwitch;
    public void Initialize(CinemachineVirtualCamera cam, GameplayUIController uiController)
    {
        camera = cam;
        ui = uiController;
    }

    public void AddFollower(Follower follower)
    {
        followers.Add(follower);
    }

    public void SetFollowers(List<Follower> newFollowers)
    {
        followers.Clear();
        followers.AddRange(newFollowers);
    }
    public void SwitchTo(int index)
    {
        if (index < 0 || index >= followers.Count) return;
        if (IsDead(index)) return;

        currentFollowerIndex = index;
        var target = followers[index].transform;
        globalTargetPlayer = target;
        camera.Follow = target;

        var characterService = followers[index].GetComponent<PlayerController>().playerData;
        ui.characterListUI.hotbar1.GetComponent<CharacterDetails>().Initialize(characterService);

        for (int i = 0; i < followers.Count; i++)
        {
            if (i == index)
                followers[i].SetTarget();
            else
                followers[i].Refresh();
        }

        OnFollowerSwitch?.Invoke();
        Debug.Log($"[FollowerService] Switched to follower {index + 1}");
    }
    public int GetAvailableFollower()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            if (!IsDead(i)) return i;
        }
        return -1;
    }

    public bool IsDead(int index)
    {
        return followers[index].GetComponent<PlayerController>().playerData.isDead;
    }

    public Transform GetCurrentTarget()
    {
        return globalTargetPlayer;
    }
}
