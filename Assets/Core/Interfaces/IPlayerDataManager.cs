public interface IPlayerDataManager
{
    SaveData[] loadedSlots { get; }
    int loadedSlotIndex { get; }
    SaveData loadedData { get; }

    void Initialize();
    void LoadSlotData();
    SaveData LoadPlayerData(int slotIndex, SaveData data);
    void SaveToSlot(int slotIndex, SaveData data);
    void DeleteSlot(int slot);
    void AutoSaveTrigger();
}
