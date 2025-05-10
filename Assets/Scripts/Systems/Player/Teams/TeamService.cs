using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamService
{
    private Team data;
    private int maxSize = 4;
    public bool isActive;
    public TeamService(Team data = null)
    {
        if (data != null)
        {
            this.data = data;
        }
        else
        {
            this.data = new Team("Team x");
        }
    }

    public void SetTeamName(string name)
    {
        data.teamName = name;
    }
    public string GetTeamName()
    {
        return data.teamName;
    }
    public void AddCharacter(int slotIndex, CharacterService data)
    {
        if (slotIndex < 0 || slotIndex >= maxSize)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }
        int currentIndex = this.data.characters.IndexOf(data);
        if (currentIndex == -1)
        {
            if (this.data.characters[slotIndex] == null)
            {
                this.data.characters[slotIndex] = data;
            }
        }

        // Case 2: Character exists, and we want to move it
        if (currentIndex != slotIndex)
        {
            if (this.data.characters[slotIndex] == null)
            {
                this.data.characters[slotIndex] = data;
                this.data.characters[currentIndex] = null;

            }
            else
            {
                CharacterService temp = this.data.characters[slotIndex];
                this.data.characters[slotIndex] = data;
                this.data.characters[currentIndex] = temp;
            }
        }
    }
    public void RemoveCharacter(CharacterService character)
    {
        int index = data.characters.IndexOf(character);
        if (index != -1)
        {
            data.characters[index] = null;
            Debug.Log($"Removed character from slot {index}");
        }
        else
        {
            Debug.LogWarning("Character not found in team.");
        }
    }
    public bool isCharacterInTeam(CharacterService data)
    {
        return this.data.characters.IndexOf(data) != -1;
    }
    public List<CharacterService> GetMembers()
    {
        return data.characters;
    }
}
