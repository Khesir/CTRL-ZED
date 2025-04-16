using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstance : MonoBehaviour
{
    public CharacterData template;

    public int currentHealth;
    public int level = 1;

    public CharacterInstance(CharacterData data)
    {
        template = data;
        currentHealth = data.baseHealth;
    }

    public int GetAttack()
    {
        return template.baseAttack + level; // Example scaling
    }
}
