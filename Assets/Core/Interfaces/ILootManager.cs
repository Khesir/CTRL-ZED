using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootManager
{
    void RegisterLoot(LootCollect loot);
    void UnregisterLoot(LootCollect loot);
}
