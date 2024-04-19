using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rhythmGameManager : MonoBehaviour
{
    private bool gameActive = false;
    public bool gameComplete = false;
    public GameObject placeholderText;
    public openGate opengate;
    public Door_Script doorfloor;
    public Door_Script door1stfloor;
    public lootKeyToHelicopter lootkeytohelicopter;

    void Start()
    {
        placeholderText.SetActive(false);
    }

    void Update()
    {
        if (gameActive)
        {
            placeholderText.SetActive(true);
        }
    }

    //PLACEHOLDER TO TRIGGER GAME COMPLETE
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && opengate.hasTriggered)
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
        doorfloor.doorLocked = false;
        door1stfloor.doorLocked = false;
        lootkeytohelicopter.keyLootActive();
        Debug.Log("rhythmGameComplete");
    }
}
