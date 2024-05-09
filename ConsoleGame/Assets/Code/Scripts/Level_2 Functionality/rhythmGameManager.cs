using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rhythmGameManager : MonoBehaviour
{
    private bool gameActive = false;
    public bool gameComplete = false;
    //public GameObject placeholderText;
    public openGate opengate;
    public Door_Script doorfloor;
    public Door_Script door1stfloor;
    public lootKeyToHelicopter lootkeytohelicopter;
    public rhythmGameCamera rhythmgamecamera;

    public GameObject rhythmGame;

    public int counter = 0;
    public int targetCounter = 3;

    void Start()
    {
        //placeholderText.SetActive(false);
        rhythmGame.SetActive(false);
    }

    void Update()
    {
        if (gameActive)
        {
            //placeholderText.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && opengate.hasTriggered)
        {
            //rhythmGameComplete();
            rhythmGame.SetActive(true);
            rhythmgamecamera.switchCamera();
        }
    }

    public void IncrementCounter()
    {
        counter++; 
        Debug.Log("Counter incremented to: " + counter); 

        if (counter >= targetCounter)
        {
            rhythmGameComplete();
        }
    }


    public void rhythmGameActive()
    {
        gameActive = true;
        Debug.Log("rhythmGameActive");
    }

    public void rhythmGameComplete()
    {
        gameComplete = true;
        rhythmgamecamera.switchCameraBack();
        rhythmGame.SetActive(false);
        doorfloor.doorLocked = false;
        door1stfloor.doorLocked = false;
        lootkeytohelicopter.keyLootActive();
        Debug.Log("rhythmGameComplete");
    }
}
