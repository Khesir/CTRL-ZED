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
}