using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLevel : MonoBehaviour
{
    public rhythmGameManager rhythmgamemanager;
    public string sceneName;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && rhythmgamemanager.gameComplete)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
