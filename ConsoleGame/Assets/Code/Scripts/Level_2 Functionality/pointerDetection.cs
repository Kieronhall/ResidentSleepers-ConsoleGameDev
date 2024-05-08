using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class pointerDetection : MonoBehaviour
{
    private GameObject _player;
    private PlayerInput _playerInput;
    private PlayerControls _playerControls;
    public rhythmGameManager rythmgamemanager;
    public pointerRotation pointerrotation;

    private FMOD.Studio.EventInstance rhythmgamesound;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerControls = _player.GetComponent<PlayerControls>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("rhythmHitPoint") && _playerControls.interact /*Input.GetKeyDown(KeyCode.E*/)
        {
            Destroy(gameObject);
            PlayMinigameSound();
            Debug.Log("Keypress + Hit Successful!");
            rythmgamemanager.IncrementCounter();
            pointerrotation.SwitchRotation();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("rhythmHitPoint"))
        {
           // Debug.Log("Collision with rhythm hit point detected!");
        }
    }

    public void PlayMinigameSound()
    {
        rhythmgamesound = FMODUnity.RuntimeManager.CreateInstance("event:/Kieron/rhythmMinigameSound");
        rhythmgamesound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        rhythmgamesound.start();
        rhythmgamesound.release();
    }

}
