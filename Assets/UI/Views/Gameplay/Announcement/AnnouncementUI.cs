using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AnnouncementUI : MonoBehaviour
{
    public TMP_Text message;
    public int duration = 3;
    public async void PushMessage(string message)
    {
        this.message.text = message;
        gameObject.SetActive(true); // this should trigger the panelAnimator-- AnimateOut
        await UniTask.Delay(duration * 1000);
        gameObject.SetActive(false); // this should trigger the panelAnimator-- AnimateOut
    }
}
