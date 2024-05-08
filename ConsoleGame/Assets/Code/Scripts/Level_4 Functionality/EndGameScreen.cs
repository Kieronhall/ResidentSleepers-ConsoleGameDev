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
            menuManager.EndGameScreen();
        }
    }
}
