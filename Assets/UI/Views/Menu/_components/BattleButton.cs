using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButton : MonoBehaviour
{
    public LevelUIManager levelUIManager;
    public void PushMessage()
    {
        levelUIManager.gameObject.SetActive(true);
        var x = levelUIManager.GetComponent<Animator>();
        x.SetTrigger("Close");
    }
}
