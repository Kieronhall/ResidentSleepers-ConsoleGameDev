using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void openLevel ()
    {
        //Load level
        Application.LoadLevel("TestingScene");
    }

    public void closeLevel ()
    {
        Application.Quit();
    }
}
