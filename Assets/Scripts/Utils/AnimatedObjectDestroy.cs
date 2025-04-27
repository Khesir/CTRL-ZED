using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObjectDestroy : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Update is called once per frame
    void Start()
    {
        Destroy(transform.parent.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
