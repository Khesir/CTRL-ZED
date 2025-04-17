using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstance
{
    public CharacterData template;

    public int currentHealth;
    public int level = 1;

    public CharacterInstance(CharacterData templateData)
    {
        template = templateData;
        currentHealth = template.baseHealth;
        level = 1;
    }
    public int GetAttack()
    {
        return template.baseAttack + level; // Example scaling
    }
}
