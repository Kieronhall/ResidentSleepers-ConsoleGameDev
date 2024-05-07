using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPerson;
using UnityEngine.InputSystem;

public class Backdoor_Script : MonoBehaviour
{
    private GameObject _player;
    private PlayerInput _playerInput;
    private PlayerControls _playerControls;

    public GameObject backdoor;
    public bool unlockBackdoor;
    public bool unlockOnce;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerControls = _player.GetComponent<PlayerControls>();

        unlockBackdoor = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (_playerControls.interact && Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 2f) 
        {
            Debug.Log("Backdoor Unlocked!");
            UnlockBackdoor();
        }

        
    }

    public void UnlockBackdoor()
    {
        Debug.Log("Backdoor Unlocked!");
        backdoor.GetComponent<Door_Script>().LockDoor(0);
        GameObject.FindGameObjectWithTag("AIController").GetComponent<Ai_Controller>().InsideAgentsOn();
    }
}
