using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButton : MonoBehaviour
{
    public LevelUIManager levelUIManager;
    public void PushMessage()
    {
        var x = levelUIManager.GetComponent<Animator>();
        x.SetTrigger("Close");
        levelUIManager.gameObject.SetActive(true);
    }
}
