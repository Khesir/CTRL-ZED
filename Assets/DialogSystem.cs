using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text dialogText;

    [Header("Dialog Settings")]
    [SerializeField] private float textSpeed = 0.03f; // Speed per character
    [SerializeField] private float autoNextDelay = 1.5f;

    private Queue<string> dialogQueue = new Queue<string>();
    private bool isPlaying = false;
    public void Start()
    {
        if (GameManager.Instance.LevelManager.activeLevel.levelID != "0") return;
        var lines = new List<string>
        {
            "Hello there!",
            "Welcome to CTRL-ZED.",
            "You Can Explore this level and try things out!",
            "Good luck!",
            "Oh, before I forget",
            "I'll be brief about this",
            "You can press 1,2,3,4 to switch characters",
            "and Activate Skills by pressing",
            "Q and E",
            "Lastly, Try to finish the game before reaching 0 OS HP",
            "Once again,",
            "Best of luck!"
        };
        StartDialog(lines).Forget();
    }
    public async UniTaskVoid StartDialog(List<string> lines)
    {
        if (isPlaying) return;
        isPlaying = true;

        dialogQueue.Clear();
        foreach (var line in lines)
            dialogQueue.Enqueue(line);

        while (dialogQueue.Count > 0)
        {
            string nextLine = dialogQueue.Dequeue();
            await ShowLine(nextLine);
            await UniTask.Delay((int)(autoNextDelay * 1000));
        }
        dialogText.text = "";
        isPlaying = false;
    }
    private async UniTask ShowLine(string line)
    {
        dialogText.text = "";
        foreach (char c in line)
        {
            dialogText.text += c;
            await UniTask.Delay((int)(textSpeed * 1000));
        }
    }
}
