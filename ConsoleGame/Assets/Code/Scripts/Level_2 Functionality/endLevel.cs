using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLevel : MonoBehaviour
{
    public rhythmGameManager rhythmgamemanager;
    public string sceneName;


    public void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if(rhythmgamemanager != null)
        {
            if (other.CompareTag("Player") && rhythmgamemanager.gameComplete)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        
        if (other.CompareTag("Player") && sceneName == "Level_2")
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
