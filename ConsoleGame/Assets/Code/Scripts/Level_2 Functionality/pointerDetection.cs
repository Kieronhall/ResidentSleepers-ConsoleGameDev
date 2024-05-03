using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class pointerDetection : MonoBehaviour
{
    private GameObject _player;
    private PlayerInput _playerInput;
    private PlayerControls _playerControls;
    public rhythmGameManager rythmgamemanager;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerControls = _player.GetComponent<PlayerControls>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("rhythmHitPoint") && /*_playerControls.interact*/ Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
            Debug.Log("Keypress + Hit Successful!");
            rythmgamemanager.IncrementCounter();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("rhythmHitPoint"))
        {
           // Debug.Log("Collision with rhythm hit point detected!");
        }
    }
}
