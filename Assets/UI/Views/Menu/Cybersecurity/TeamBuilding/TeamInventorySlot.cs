using UnityEngine;
using UnityEngine.EventSystems;
public class TeamInventorySlot : MonoBehaviour, IDropHandler
{
    public GameObject draggableItemPrefab;
    public int slotIndex;
    public TeamService teamService;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount > 0) return;

        GameObject dropped = eventData.pointerDrag;
        DraggableItem originalDraggable = dropped.GetComponent<DraggableItem>();
        CharacterData transferredCharacter = originalDraggable.instance;
        var manager = GameManager.Instance.TeamManager;
        if (originalDraggable.isExternal)
        {
            // Validate if character is not duplicate
            if (!manager.isCharacterInTeam(teamService.GetData().teamID, transferredCharacter).IsSuccess) return;

            manager.AssignedCharacterToSlot(teamService.GetData().teamID, slotIndex, transferredCharacter);
        }
        else
        {
            TeamInventorySlot originalSlot = originalDraggable.parentAfterDrag.GetComponent<TeamInventorySlot>();
            if (teamService.GetData().teamID != originalSlot.teamService.GetData().teamID) return;
            manager.AssignedCharacterToSlot(teamService.GetData().teamID, slotIndex, transferredCharacter);
            manager.RemoveCharacterFromSlot(originalSlot.teamService.GetData().teamID, originalSlot.slotIndex);

            originalDraggable.parentAfterDrag = transform;
        }
        ServiceLocator.Get<ISoundService>().Play(SoundCategory.Team, SoundType.Team_OnDrop);
    }
}
