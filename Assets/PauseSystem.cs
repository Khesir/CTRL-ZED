using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public GameObject pauseMenu;
    public PanelAnimator panel;
    [SerializeField] private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    public async void PauseGame()
    {
        Debug.Log("Game Paused");
        pauseMenu.SetActive(true);
        await panel.Show();
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        panel.Hide(pauseMenu).Forget();

        Time.timeScale = 1f;
        isPaused = false;
    }
}
