using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompleteScreenUI : MonoBehaviour
{
    public Animator animator;
    public TMP_Text title;
    public TMP_Text message;
    public TMP_Text reward;

    // Include resources plundered during battle
    public void Complete(string type, bool complete, string team = "")
    {
        switch (type)
        {
            case "os":
                title.text = "";
                message.text = $"OS HP runned out!";
                break;
            case "character":
                if (complete)
                {
                    title.text = "Wave Cleared!";
                    message.text = $"{team} has succesfully! Stopped the wave";
                }
                else
                {
                    title.text = "Game Over";
                    message.text = $"{team} has been wiped! Go back to base to repair or use other team";
                }
                break;
        }
        animator.SetTrigger("Complete");
    }
}
