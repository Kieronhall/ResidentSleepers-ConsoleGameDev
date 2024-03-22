using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fsmAgentAnimationState : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
}

