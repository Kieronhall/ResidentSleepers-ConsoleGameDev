using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Players
    public GameObject Player_1;
    public GameObject Player_2;

    //SpawnPoint
    public Transform spawnPoint;

    //BackDoorMinigame
    public Canvas Canvas;
    public GameObject ElectricPanel;
    public GameObject BackDoor;
    public Button startBtn;
    public Image map;

    // Start is called before the first frame update
    void Start()
    {
        //Place Player 1 and Start Point
        Player_1.transform.position = spawnPoint.position;

        //MiniGame
        Canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(ElectricPanel.transform.position, Player_1.transform.position) <= 4f)
        {
            Debug.Log("EnableMiniGame!");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Canvas.gameObject.SetActive(true);
            }
        }
        startBtn.onClick.AddListener(StartMinigame);
        startBtn.Select();
    }

    void UnlockBackDoor()
    {
        BackDoor.GetComponent<Door_Script>().LockDoor(2);
    }
    
    void StartMinigame()
    {
        map.gameObject.SetActive(true);
        Debug.Log("start minigame!");
    }
}
