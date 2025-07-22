using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public GameObject LevelPrefab;
    public Transform content;
    public LevelInformationModal levelInformationModal;
    void OnDisable()
    {
        // #A500FF // Active Color button
        Clear();
    }
    public void Generate()
    {
        var levels = GameManager.Instance.LevelManager.allLevels;
        Clear();
        for (int i = 0; i < levels.Count - 1; i++)
        {
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
