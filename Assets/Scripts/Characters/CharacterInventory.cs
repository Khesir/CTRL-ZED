using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public static CharacterInventory Instance { get; private set; }

    public List<CharacterInstance> ownedCharacters = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ownedCharacters = new List<CharacterInstance>();
    }
    public void AddCharacter(CharacterData data)
    {
        var instance = new CharacterInstance(data);
        ownedCharacters.Add(instance);
    }
}
