using FMODUnity;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLevel : MonoBehaviour
{
    public ASyncLoader loader;
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
            var masterBus = RuntimeManager.GetBus("bus:/");
            masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            loader.LoadLevelButton(sceneName);
        }
    }

}
