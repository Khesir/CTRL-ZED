using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AntiVirusManager : MonoBehaviour
{
    public static AntiVirusManager Instance { get; private set; }
    [Header("Assigned via Inspector or Resources.Load")]
    public List<AntiVirus> allBuffs = new();
    public List<AntiVirus> items;
    private Dictionary<string, AntiVirus> buffLookup;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public async UniTask Initialize(int level = -1)
    {
        if (allBuffs.Count == 0)
            allBuffs.AddRange(Resources.LoadAll<AntiVirus>("AntiVirusBuffs"));

        // Build lookup from filtered list
        buffLookup = allBuffs.ToDictionary(buff => buff.buffID);

        Debug.Log("[AntiVirusManager] Anti-Virus Manager Initialized");
        await UniTask.CompletedTask;
    }
    public AntiVirus GetBuffByID(string id)
    {
        return buffLookup.TryGetValue(id, out var data) ? data : null;
    }
    public List<AntiVirus> GetAllBuffs() => allBuffs;
}
