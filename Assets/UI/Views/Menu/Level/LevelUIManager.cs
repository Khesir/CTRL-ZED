using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public GameObject LevelPrefab;
    public Transform content;
    public LevelInformationModal levelInformationModal;
    public PanelAnimator panel;
    public CanvasGroup canvasGroup;
    private bool isActive = false;
    public void TriggerArena()
    {
        if (!isActive)
        {
            isActive = true;
            ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_OnOpen);
            Generate();
        }
        else
        {

            isActive = false;
            ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_OnClose);
            Clear();
            canvasGroup.alpha = 0;
        }
    }
    public async void Generate()
    {
        await panel.Show();

        var levels = ServiceLocator.Get<ILevelManager>().allLevels;
        Debug.Log(levels.Count);
        Clear();
        for (int i = 0; i < levels.Count - 1; i++)
        {
            if (int.Parse(levels[i].levelID) == 0) continue;
            var go = Instantiate(LevelPrefab, content);
            var lvl = go.GetComponent<LevelPrefab>();
            lvl.Setup(levels[i]);
            lvl.modalComponent = levelInformationModal;
        }

    }
    public void Clear()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
