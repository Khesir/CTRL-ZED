using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PaginationUI paginationUI;
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private List<GameObject> allItems = new List<GameObject>();
    [SerializeField] private Button exitbutton;
    public int currentIndex = 0;
    void Start()
    {
        exitbutton.interactable = false;
        foreach (GameObject item in allItems)
        {
            if (item != null)
                item.SetActive(false);
        }

        paginationUI.SetTotalItems(allItems.Count);

        paginationUI.OnPageChanged.AddListener(OnPageChanged);
        OnPageChanged(1);
    }
    private void OnPageChanged(int pageNumber)
    {
        Debug.Log($"Page changed to: {pageNumber}");

        // Hide current item
        if (currentIndex >= 0 && currentIndex < allItems.Count && allItems[currentIndex] != null)
        {
            allItems[currentIndex].SetActive(false);
        }

        // Convert page number (1-based) to array index (0-based)
        currentIndex = pageNumber - 1;

        // Show new item
        if (currentIndex >= 0 && currentIndex < allItems.Count && allItems[currentIndex] != null)
        {
            allItems[currentIndex].SetActive(true);
        }
        if (currentIndex == allItems.Count - 1)
            exitbutton.interactable = true;
    }
    private void OnDestroy()
    {
        // Clean up listener
        if (paginationUI != null)
        {
            paginationUI.OnPageChanged.RemoveListener(OnPageChanged);
        }
    }
}
