using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public SaveData[] loadedSlots = new SaveData[3];
    public void Initialize()
    {
        LoadSlotData();
    }
    public void LoadSlotData()
    {
        for (int i = 0; i < loadedSlots.Length; i++)
        {
            loadedSlots[i] = SaveSystem.LoadFromSlot(i + 1);
        }
    }
    public SaveData LoadPlayerData(int slotIndex)
    {
        return SaveSystem.LoadFromSlot(slotIndex + 1);
    }
    public void SaveToSlot(int slotIndex, SaveData data)
    {
        SaveSystem.SaveToSlot(slotIndex + 1, data);
        LoadSlotData();
    }
}
