using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [Header("Menu References")]
    public MenuUIController menuUIController;
    public bool isGameActive;
    public bool _isInitialized = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public async UniTask Initialize()
    {
        await UniTask.WaitUntil(() => GameManager.Instance != null && GameManager.Instance._isInitialized);
        if (_isInitialized) return;

        isGameActive = false;

        _isInitialized = true;
        await UniTask.CompletedTask;
    }
}
