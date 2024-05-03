using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backdoor_Script : MonoBehaviour
{
    public bool DoorUnlocked = false;

    public bool PlayerInside = false;

    public GameObject Exterior_Assets;
    public GameObject Interior_Assets;
    public GameObject AiController;

    public bool MinigameComplete = false;

    public bool playerEnterring = false;

    // Start is called before the first frame update
    void Start()
    {
        Interior_Assets.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DoorUnlocked && PlayerInside)
        {
            Exterior_Assets.SetActive(false);
           
        }


        if (MinigameComplete)
        { 
            
        }
    }

    public void UnlockBackDoor()
    {
        DoorUnlocked = true;
        Interior_Assets.SetActive(true);
        this.gameObject.GetComponent<Door_Script>().LockDoor(0);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entering Building!");
            playerEnterring = true;
            AiController.GetComponent<Ai_Controller>().InsideAgentsOn();


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&& playerEnterring)
        { 
            Debug.Log("Player Inside Close Door and Unload Exterior");
            PlayerInside = true;
            
        }
    }
}
