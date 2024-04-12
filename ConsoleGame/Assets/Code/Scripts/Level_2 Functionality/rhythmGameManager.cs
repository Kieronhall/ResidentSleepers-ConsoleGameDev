using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rhythmGameManager : MonoBehaviour
{
    private bool gameActive = false;
    public bool gameComplete = false;
    public GameObject placeholderText;
    public openGate opengate;

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
        Debug.Log("rhythmGameComplete");
    }
}
