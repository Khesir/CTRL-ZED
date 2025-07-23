using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Animator logo;
    void OnEnable()
    {
        logo.SetTrigger("Float");
    }
    void OnDisable()
    {
        logo.SetTrigger("Float");
    }
}
