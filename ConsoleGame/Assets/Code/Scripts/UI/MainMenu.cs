using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void openLevel ()
    {
        //Load level
        SceneManager.LoadScene("Level_4 - Beginning");
        Debug.Log("Opening scene");
    }

    public void closeLevel ()
    {
        Application.Quit();
    }
}
