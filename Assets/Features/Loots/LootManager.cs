using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] private List<LootCollect> activeLoots = new List<LootCollect>();
    [SerializeField] private Transform player;

    void Update()
    {
        var currentTarget = GameplayManager.Instance.followerManager.GetCurrentTarget();
        if (player != currentTarget)
            player = currentTarget;

        if (player == null) return;

        foreach (var loot in activeLoots)
        {
            loot.TryAttract(player);
        }
    }

    public void RegisterLoot(LootCollect loot)
    {
        activeLoots.Add(loot);
    }

    public void UnregisterLoot(LootCollect loot)
    {
        activeLoots.Remove(loot);
    }
}
