using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoader : MonoBehaviour
{
    public GameObject prefab;
    public Transform container;
    void OnEnable()
    {
        Refresh();
        CoreEventBus.Subscribe<SaveSystemDeleteActionEvent>(Refresh);
    }
    void OnDisable()
    {
        CoreEventBus.Unsubscribe<SaveSystemDeleteActionEvent>(Refresh);
    }
    private void Refresh(SaveSystemDeleteActionEvent evt) => Refresh();
    private void Refresh()
    {
        ClearContainer();

        var saves = ServiceLocator.Get<IPlayerDataManager>().loadedSlots;


        for (int i = 0; i < saves.Length; i++)
        {
            var go = Instantiate(prefab, container);
            var saveContainer = go.GetComponent<SaveContainer>();
            saveContainer.Initialize(saves[i], i);
        }
    }
    private void ClearContainer()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}
