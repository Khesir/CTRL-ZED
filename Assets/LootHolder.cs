using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootHolder : MonoBehaviour
{
    public LootCounter money;
    public LootCounter food;
    public LootCounter technology;
    public LootCounter energy;
    public LootCounter intelligence;
    private Dictionary<ItemType, LootCounter> counters;

    public void Setup()
    {
        counters = new Dictionary<ItemType, LootCounter>
    {
        { ItemType.Food, food },
        { ItemType.Coins, money },
        { ItemType.Technology, technology },
        { ItemType.Energy, energy },
        { ItemType.Intelligence, intelligence }
    };

        foreach (var counter in counters.Values)
            counter.UpdateCounter();
    }

    public void AddAmount(LootDropData dropData)
    {
        if (counters.TryGetValue(dropData.item.itemType, out var counter))
            counter.UpdateCounter(dropData);
    }
    public int GetLoot(ItemType type)
    {
        if (counters.TryGetValue(type, out var counter))
        {
            return counter.currentAmount;
        }
        return 0;
    }
}
