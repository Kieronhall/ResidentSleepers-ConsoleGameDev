using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndGameScreen : MonoBehaviour
{
    public MenuManager menuManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var masterBus = RuntimeManager.GetBus("bus:/");
            masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            menuManager.EndGameScreen();
        }
    }
}
