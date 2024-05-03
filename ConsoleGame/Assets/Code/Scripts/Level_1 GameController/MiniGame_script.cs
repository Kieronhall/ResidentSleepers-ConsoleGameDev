using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame_script : MonoBehaviour
{

    public GameObject Player;
    public GameObject BackDoor;
    // Start is called before the first frame update
    void Start()
    {
        Player= GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 5f)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                BackDoor.GetComponent<Backdoor_Script>().UnlockBackDoor();
                Debug.Log("BackDoor Unlocked!");
            }
        }
    }
}
