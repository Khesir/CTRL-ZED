using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class LevelPrefab : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text objective;
    public Image disabled;
    public LevelInformationModal modalComponent;
    private LevelData data;
    public void Setup(LevelData data)
    {
        this.data = data;
        var button = GetComponent<Button>();
        title.text = data.levelName;
        icon.sprite = data.levelIcon;
        objective.text = data.objective;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Active);

        // gameplayButton.onClick.RemoveAllListeners();
        // if (active)
        // {
        //     disabled.gameObject.SetActive(false);
        //     gameplayButton.interactable = true;
        //     gameplayButton.onClick.AddListener(() => StartGameplay(index));
        // }
        // else
        // {
        //     disabled.gameObject.SetActive(true);
        //     gameplayButton.interactable = false;
        // }
    }
    public void Active()
    {
        modalComponent.data = data;
        modalComponent.Trigger();
        ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_OnOpen);
    }
}
