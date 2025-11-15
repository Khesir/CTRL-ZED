using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PaginationUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button lastButton;
    [SerializeField] private TextMeshProUGUI pageInfoText;
    [SerializeField] private Transform pageButtonsContainer;

    [Header("Settings")]
    [SerializeField] private int totalItems = 100;
    [SerializeField] private int itemsPerPage = 10;
    [SerializeField] private int maxVisiblePageButtons = 5;
    [SerializeField] private GameObject pageButtonPrefab;

    [Header("Styling")]
    [SerializeField] private Color normalButtonColor = Color.white;
    [SerializeField] private Color activeButtonColor = Color.cyan;
    [SerializeField] private Color disabledButtonColor = Color.gray;

    public UnityEvent<int> OnPageChanged = new UnityEvent<int>();

    // Private variables
    private int currentPage = 1;
    private int totalPages;
    private List<Button> pageButtons = new List<Button>();

    private void Start()
    {
        CalculateTotalPages();
        SetupButtonListeners();
        UpdateUI();
    }
    private void CalculateTotalPages()
    {
        totalPages = Mathf.CeilToInt((float)totalItems / itemsPerPage);
    }
    private void SetupButtonListeners()
    {
        if (previousButton != null) previousButton.onClick.AddListener(GoToPreviousPage);
        if (nextButton != null) nextButton.onClick.AddListener(GoToNextPage);
        if (firstButton != null) firstButton.onClick.AddListener(GoToFirstPage);
        if (lastButton != null) lastButton.onClick.AddListener(GoToLastPage);
    }
    public void GoToPage(int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > totalPages) return;

        currentPage = pageNumber;
        UpdateUI();
        OnPageChanged?.Invoke(currentPage);
    }
    private void GoToNextPage()
    {
        if (currentPage < totalPages)
        {
            GoToPage(currentPage + 1);
        }
    }
    public void GoToPreviousPage()
    {
        if (currentPage > 1)
        {
            GoToPage(currentPage - 1);
        }
    }
    public void GoToFirstPage()
    {
        GoToPage(1);
    }
    public void GoToLastPage()
    {
        GoToPage(totalPages);
    }
    private void UpdateUI()
    {
        UpdateNavigationButtons();
        UpdatePageButtons();
        UpdatePageInfoText();
    }
    private void UpdateNavigationButtons()
    {
        if (previousButton != null)
        {
            previousButton.interactable = currentPage > 1;
            SetButtonColor(previousButton, currentPage > 1 ? normalButtonColor : disabledButtonColor);
        }

        if (nextButton != null)
        {
            nextButton.interactable = currentPage < totalPages;
            SetButtonColor(nextButton, currentPage < totalPages ? normalButtonColor : disabledButtonColor);
        }

        if (firstButton != null)
        {
            firstButton.interactable = currentPage > 1;
            SetButtonColor(firstButton, currentPage > 1 ? normalButtonColor : disabledButtonColor);
        }

        if (lastButton != null)
        {
            lastButton.interactable = currentPage < totalPages;
            SetButtonColor(lastButton, currentPage < totalPages ? normalButtonColor : disabledButtonColor);
        }
    }

    private void UpdatePageButtons()
    {
        if (pageButtonsContainer == null || pageButtonPrefab == null)
            return;

        // Clear existing buttons
        foreach (Button btn in pageButtons)
        {
            if (btn != null)
                Destroy(btn.gameObject);
        }
        pageButtons.Clear();

        // Calculate which page buttons to show
        int startPage, endPage;
        CalculateVisiblePageRange(out startPage, out endPage);

        // Create page buttons
        for (int i = startPage; i <= endPage; i++)
        {
            CreatePageButton(i);
        }
    }
    private void CalculateVisiblePageRange(out int startPage, out int endPage)
    {
        int halfVisible = maxVisiblePageButtons / 2;
        startPage = Mathf.Max(1, currentPage - halfVisible);
        endPage = Mathf.Min(totalPages, startPage + maxVisiblePageButtons - 1);

        // Adjust if we're near the end
        if (endPage - startPage + 1 < maxVisiblePageButtons)
        {
            startPage = Mathf.Max(1, endPage - maxVisiblePageButtons + 1);
        }
    }
    private void CreatePageButton(int pageNumber)
    {
        GameObject buttonObj = Instantiate(pageButtonPrefab, pageButtonsContainer);
        Button button = buttonObj.AddComponent<Button>();

        if (button != null)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = pageNumber.ToString();
            }

            // Add click listener
            int page = pageNumber; // Capture for lambda
            button.onClick.AddListener(() => GoToPage(page));

            // Set button color based on whether it's the current page
            bool isCurrentPage = pageNumber == currentPage;
            SetButtonColor(button, isCurrentPage ? activeButtonColor : normalButtonColor);

            pageButtons.Add(button);
        }
    }
    private void UpdatePageInfoText()
    {
        if (pageInfoText != null)
        {
            int startItem = (currentPage - 1) * itemsPerPage + 1;
            int endItem = Mathf.Min(currentPage * itemsPerPage, totalItems);

            pageInfoText.text = $"{currentPage}/{totalItems}";
        }
    }

    private void SetButtonColor(Button button, Color color)
    {
        if (button != null)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = color;
            button.colors = colors;
        }
    }
    public void SetTotalItems(int items)
    {
        totalItems = items;
        CalculateTotalPages();

        // Reset to first page if current page is now out of range
        if (currentPage > totalPages)
        {
            currentPage = 1;
        }

        UpdateUI();
    }

    public void SetItemsPerPage(int items)
    {
        itemsPerPage = items;
        CalculateTotalPages();

        // Reset to first page
        currentPage = 1;
        UpdateUI();
    }

    public int GetCurrentPage()
    {
        return currentPage;
    }

    public int GetTotalPages()
    {
        return totalPages;
    }

    public void GetCurrentPageRange(out int startIndex, out int endIndex)
    {
        startIndex = (currentPage - 1) * itemsPerPage;
        endIndex = Mathf.Min(startIndex + itemsPerPage - 1, totalItems - 1);
    }
}
