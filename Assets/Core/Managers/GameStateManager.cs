using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    None,
    MainMenu,
    Gameplay,
    Credits,
    Loading
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState Currentstate { get; private set; } = GameState.None;
    public event Action<GameState> OnStateChanged;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetState(GameState newState)
    {
        if (newState == Currentstate) return;
        Currentstate = newState;
        Debug.Log($"[GameStateManager] State changed to: {newState}");
        OnStateChanged.Invoke(newState);
        switch (newState)
        {
            case GameState.MainMenu:
                SceneLoader.LoadScene("MainMenuScene");
                break;

            case GameState.Gameplay:
                SceneLoader.LoadScene("GameplayScene");
                break;

            case GameState.Credits:
                SceneLoader.LoadScene("CreditsScene");
                break;

            case GameState.Loading:
                // Optionally use for loading screen scene
                break;
        }
    }
}
