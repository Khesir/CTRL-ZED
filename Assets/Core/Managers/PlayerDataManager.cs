using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public SaveData[] loadedSlots = new SaveData[3];
    public int loadedSlotIndex = -1;
    public SaveData loadedData;
    public void Initialize()
    {
        LoadSlotData();
        Debug.Log("Save path: " + Application.persistentDataPath);
    }
    public void LoadSlotData()
    {
        for (int i = 0; i < loadedSlots.Length; i++)
        {
            loadedSlots[i] = SaveSystem.LoadFromSlot(i + 1);
        }
    }
    public SaveData LoadPlayerData(int slotIndex, SaveData data)
    {
        if (data == null)
        {
            loadedSlotIndex = -1;
            loadedData = null;
        }
        else
        {
            loadedSlotIndex = slotIndex + 1;
            loadedData = data;
        }
        return data;
    }
    public void SaveToSlot(int slotIndex, SaveData data)
    {
        SaveSystem.SaveToSlot(slotIndex + 1, data);
        LoadSlotData();
    }
    public void DeleteSlot(int slot)
    {
        SaveSystem.DeleteSlot(slot + 1);
        LoadSlotData();
    }
    public void AutoSaveTrigger()
    {
        if (loadedSlotIndex <= 0 || loadedData == null)
        {
            Debug.LogWarning("[PlayerDataManager] No valid slot loaded, autosave skipped");
            return;
        }

        SaveSystem.SaveToSlot(loadedSlotIndex, loadedData);
        LoadSlotData();
        Debug.Log("[PlayerDataManager] Auto save triggered");
    }
}
