using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public static class SaveSystem
{
    public static event Action onSaveAction;
    static public void SaveToSlot(int slot, SaveData data)
    {
        string path = Application.persistentDataPath + $"/slot{slot}.json";
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
        CoreEventBus.Publish(new SaveSystemOnLoadEvent());
    }
    static public SaveData LoadFromSlot(int slot)
    {
        string path = Application.persistentDataPath + $"/slot{slot}.json";
        if (File.Exists(path))
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        return null;
    }
    static public void DeleteSlot(int slot)
    {
        string path = GetPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted save slot {slot} at {path}");
        }
        else
        {
            Debug.LogWarning($"No save file found at slot {slot}");
        }
    }
    private static string GetPath(int slot)
       => Application.persistentDataPath + $"/slot{slot}.json";
}
