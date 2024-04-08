using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void openLevel ()
    {
        //Load level
        SceneManager.LoadScene("MainMenu");
    }

    public void closeLevel ()
    {
        Application.Quit();
    }
}
