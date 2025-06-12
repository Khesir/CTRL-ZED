using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(string scene)
    {
        switch (scene)
        {
            case "menu":
                SceneManager.LoadScene("MainMenu");
                break;

            case "gameplay":
                SceneManager.LoadScene("Gameplay");
                break;
            case "start":
                SceneManager.LoadScene("StartMenu");
                break;
            default:
                Debug.LogWarning($"Scene '{scene}' not recognized.");
                break;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
