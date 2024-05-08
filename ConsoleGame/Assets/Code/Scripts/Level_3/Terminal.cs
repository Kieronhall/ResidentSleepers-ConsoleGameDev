using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : Interactable
{
    public ASyncLoader loader;
    public string sceneName;
    protected override void Interact()
    {
        //Loading and transitioning to next level
        loader.LoadLevelButton(sceneName);
    }
}