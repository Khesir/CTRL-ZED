using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButton : MonoBehaviour
{
    public Animator animator;
    public void PushMessage()
    {
        animator.SetTrigger("Close");
    }
}
