using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : Interactable
{
    protected override void Interact()
    {
        Debug.Log("Level Complete");
    }
}