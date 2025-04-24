using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamContainer : MonoBehaviour
{
    public Transform content;
    public TMP_Text teamName;
    public GameObject teamSlotPrefab;
    public GameObject teamDraggrable;
    public List<GameObject> teamSlot;
    public Team instance;

    public void Setup(Team team, int index)
    {
        instance = team;
        teamName.text = team.teamName;
        for (int i = 0; i < team.characters.Count; i++)
        {
            Debug.Log(team.characters.Count);
            var slot = instance.characters[i];

            var cardGO = Instantiate(teamSlotPrefab, content);
            var slotData = cardGO.GetComponent<TeamInventorySlot>();
            slotData.teamId = index;
            slotData.slotIndex = i;
            teamSlot.Add(cardGO);

            if (slot != null)
            {
                var itemGO = Instantiate(teamDraggrable, cardGO.transform);
                var draggable = itemGO.GetComponent<DraggableItem>();
                draggable.Setup(slot);
            }
        }
    }
}
