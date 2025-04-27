using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }
    public List<Follower> followers = new List<Follower>();
    [SerializeField] private int currentFollowerIndex = 0;
    public Transform globalTargetPlayer;
    public event Action switchUser;
    public CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SwitchControlledFollower(currentFollowerIndex);
    }

    void Update()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i != currentFollowerIndex)
                {
                    SwitchControlledFollower(i);
                }
            }
        }
    }
    private void SwitchControlledFollower(int newIndex)
    {
        if (newIndex >= 0 && newIndex < followers.Count)
        {
            var newLeader = followers[newIndex];

            for (int i = 0; i < followers.Count; i++)
            {
                if (i == newIndex)
                {
                    followers[i].SetTarget();
                }
                else
                {
                    followers[i].Refresh();
                }
            }

            currentFollowerIndex = newIndex;
            globalTargetPlayer = newLeader.transform;
            virtualCamera.Follow = newLeader.transform;
            switchUser.Invoke();

            Debug.Log($"Now controlling follower {currentFollowerIndex + 1}");
        }
    }
}
