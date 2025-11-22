using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour, ILootManager
{
    [SerializeField] private List<LootCollect> activeLoots = new List<LootCollect>();
    [SerializeField] private Transform player;

    void Update()
    {
        var currentTarget = ServiceLocator.Get<IFollowerManager>().GetCurrentTarget();

        if (player != currentTarget)
            player = currentTarget;

        if (player == null || activeLoots == null || activeLoots.Count == 0)
            return;

        for (int i = activeLoots.Count - 1; i >= 0; i--)
        {
            var loot = activeLoots[i];
            if (loot == null)
            {
                activeLoots.RemoveAt(i);
                continue;
            }

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
