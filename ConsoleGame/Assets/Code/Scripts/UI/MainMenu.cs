using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void openMainMenuLevel ()
    {
        //Load level
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGameLevel ()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void closeLevel ()
    {
        Application.Quit();
    }
}
