using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootKeyToHelicopter : MonoBehaviour
{
    private bool keyActive = false;
    public bool keyComplete = false;
    public rhythmGameManager rythmGameManager;
    public Door_Script doorroof;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && keyActive)
        {
            keyLootComplete();
        }
    }

    void Update()
    {
        
    }

    public void keyLootActive()
    {
        keyActive = true;
        Debug.Log("keyLootActive");
    }

    public void keyLootComplete()
    {
        keyComplete = true;
        doorroof.doorLocked = false;
        Debug.Log("keyLootComplete");
    }
}
