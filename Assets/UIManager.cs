using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Global Canvases")]
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private Canvas popupCanvas;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowLoading(bool show)
    {
        if (loadingCanvas != null)
            loadingCanvas.gameObject.SetActive(show);
    }
    public void ShowPopup(GameObject popupPrefab)
    {
        if (popupCanvas != null & popupCanvas != null)
        {
            Instantiate(popupPrefab, popupCanvas.transform);
        }
    }
}
