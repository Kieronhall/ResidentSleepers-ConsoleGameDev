using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootKeyToHelicopter : MonoBehaviour
{
    private bool keyActive = false;
    public bool keyComplete = false;
    public rhythmGameManager rythmGameManager;
    public Door_Script doorroof;

    public Vector3 rotationSpeed = new Vector3(0, 100, 0); 


    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && keyActive)
        {
            keyLootComplete();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
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
