using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationState : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
}

