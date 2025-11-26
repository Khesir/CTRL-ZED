using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Start is called before the first frame update
    // Call this method when the button is clicked
    public void ExitGame()
    {
        // If running in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If running as a built application
            Application.Quit();
#endif

        Debug.Log("Game is exiting...");
    }
}
