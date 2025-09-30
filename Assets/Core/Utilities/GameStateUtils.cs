using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateUtils
{
    public static GameState GetStateFromSceneName(string sceneName)
    {
        return sceneName switch
        {
            "MainMenu" => GameState.MainMenu,
            "Gameplay" => GameState.Gameplay,
            "Title" => GameState.TitleScreen,
            _ => GameState.Initial
        };
    }
    // New: GameState -> scene name
    public static string GetSceneNameFromState(GameState state)
    {
        return state switch
        {
            GameState.MainMenu => "MainMenu",
            GameState.Gameplay => "Gameplay",
            GameState.TitleScreen => "Title",
            GameState.Credits => "Credits",
            GameState.Initial => "InitialScene",
            _ => string.Empty
        };
    }
}