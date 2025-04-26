using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }
    public List<Follower> followers = new List<Follower>();
    private int currentFollowerIndex = 0;
    public Transform player;
    // Update is called once per frame
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
                    // This is the newly controlled follower
                    followers[i].SetTarget(followers[i].transform);
                }
                else
                {
                    // These are the followers, they should follow the new leader
                    followers[i].Refresh(newLeader.transform);
                }
            }

            currentFollowerIndex = newIndex;

            Debug.Log($"Now controlling follower {currentFollowerIndex + 1}");
        }
    }
}
