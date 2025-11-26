using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class DetailTeamMember : MonoBehaviour
{
    public Image icon;
    public TMP_Text className;
    public TMP_Text characterName;
    public TMP_Text level;
    public Button removeInstanceFromTeam;
    public DetailsController detailsController;
    public string index;
    private CharacterData instance;
    public void Initialize(CharacterData instance)
    {
        this.instance = instance;
        UpdateData();
        removeInstanceFromTeam.onClick.RemoveAllListeners();
        removeInstanceFromTeam.onClick.AddListener(RemoveMember);
    }
    public void SetToState(bool flag)
    {
        icon.gameObject.SetActive(flag);
        className.gameObject.SetActive(flag);
        characterName.gameObject.SetActive(flag);
        level.gameObject.SetActive(flag);
        removeInstanceFromTeam.gameObject.SetActive(flag);
    }
    public void UpdateData()
    {
        var character = instance;

        icon.sprite = character.baseData.icon;
        icon.color = new Color32(255, 255, 255, 255);
        className.text = character.baseData.className;
        characterName.text = character.name;
        level.text = $"Lvl. {character.level}";

    }
    private void OnActionButtonClicked()
    {
        detailsController.Intialize(instance);
    }
    private void RemoveMember()
    {
        var res = ServiceLocator.Get<ITeamManager>().RemoveCharacterFromTeamByReference(index, instance);
        if (res)
            ResetUI();
    }
    private void ResetUI()
    {
        icon.sprite = null;
        icon.color = new Color32(255, 255, 255, 0);
        className.text = "";
        characterName.text = "";
        level.text = "";
    }
}
