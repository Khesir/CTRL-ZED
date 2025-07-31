using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[Serializable]
public class FollowerService
{
    [Header("Followers")]
    [SerializeField] private List<Follower> followers = new();
    private int currentIndex = 0;

    public event Action OnFollowerSwitch;
    public void Initialize(List<Follower> followers)
    {
        SetFollowers(followers);
    }

    public void AddFollower(Follower follower)
    {
        followers.Add(follower);
    }

    public void SetFollowers(List<Follower> newFollowers)
    {
        followers = newFollowers;
        foreach (var f in followers)
        {
            var x = f.gameObject.GetComponent<PlayerGameplayService>();
            x.SetInputEnabled(false);
        }
    }

    public void SwitchTo(int index)
    {
        if (index < 0 || index >= followers.Count)
        {
            Debug.LogWarning("[FollowerService] Index out of range");
            return;
        }

        if (IsDead(index))
        {
            Debug.LogWarning("[FollowerService] Target follower is dead");
            return;
        }
        // Update Target
        for (int i = 0; i < followers.Count; i++)
        {
            bool isActive = i == index;
            // followers[i].isControlledPlayer = isActive;
            followers[i].SetTarget(!isActive ? followers[index].transform : null);

            var pgs = followers[i].GetComponent<PlayerGameplayService>();
            if (pgs != null)
                pgs.SetInputEnabled(isActive);
        }

        currentIndex = index;
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
    public Follower GetCurrentFollower() => followers[currentIndex];

    public bool IsDead(int index)
    {
        return followers[index].GetComponent<Follower>().characterData.isDead;
    }

    public void ResetTarget()
    {
        currentIndex = -1;
        OnFollowerSwitch?.Invoke();
    }
}
