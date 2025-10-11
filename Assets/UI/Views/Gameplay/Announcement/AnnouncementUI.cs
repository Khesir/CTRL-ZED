using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AnnouncementUI : MonoBehaviour
{
    public TMP_Text message;
    public int duration = 3;
    public async UniTask PushMessage(string message)
    {
        this.message.text = message;
        gameObject.SetActive(true); // Essentically this should be handled in show() but due to many panels are using this, adding this now can cause error.
        await gameObject.GetComponent<PanelAnimator>().Show();
        await UniTask.Delay(duration * 1000);
        await gameObject.GetComponent<PanelAnimator>().Hide(gameObject);
    }
}
