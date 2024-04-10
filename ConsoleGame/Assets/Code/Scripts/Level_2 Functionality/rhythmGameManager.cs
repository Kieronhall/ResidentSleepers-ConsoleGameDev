using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rhythmGameManager : MonoBehaviour
{
    private bool gameActive = false;
    public GameObject placeholderText;
    
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

    public void rhythmGameActive()
    {
        gameActive = true;
        Debug.Log("rhythmGameActive");
    }
}
