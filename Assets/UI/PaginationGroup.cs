using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaginationGroup : MonoBehaviour
{
    public List<GameObject> pages = new List<GameObject>();
    public int currentPage = 0;
    public Button nextButton;
    public Button prevButton;
    public TMP_Text pageIndicator; // optional
    public void OnEnable()
    {
        UpdatePages();

        if (nextButton)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextPage);
        }
        if (prevButton)
        {
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(PrevPage);
        }
    }
    public void ResetPages()
    {

    }
    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            UpdatePages();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }

    public void UpdatePages()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        if (pageIndicator)
        {
            pageIndicator.text = $"Page {currentPage + 1} / {pages.Count}";
        }

        if (prevButton) prevButton.interactable = currentPage > 0;
        if (nextButton) nextButton.interactable = currentPage < pages.Count - 1;
    }
    public void ResetPage()
    {
        currentPage = 0;
        UpdatePages();
    }
}
