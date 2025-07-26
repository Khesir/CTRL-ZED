using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AnnouncementUI : MonoBehaviour
{
    public TMP_Text message;
    public Animator animator;
    public int duration = 3;
    public async void PushMessage(string message)
    {
        this.message.text = message;

        animator.SetTrigger("Close");
        await UniTask.Delay(duration * 1000);
        animator.SetTrigger("Close");
    }
}
