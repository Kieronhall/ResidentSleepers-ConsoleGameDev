using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerAnimationState : MonoBehaviour
{
    public Animator animator;

    private GameObject _player;
    private PlayerInput _playerInput;
    private PlayerControls _playerControls;

    private string isWalking = "isWalking";
    private string isRunning = "isRunning";

    void Start()
    {
        animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerControls = _player.GetComponent<PlayerControls>();
    }

    void Update()
    {
        if (_playerControls.move != Vector2.zero) // no input
        {
            animator.SetBool(isWalking, true);
        }
        else if (_playerControls.move == Vector2.zero)
        {
            animator.SetBool(isWalking, false);
        }

        if (_playerControls.sprint)
        {
            animator.SetBool(isRunning, true);
        }
        else if (!_playerControls.sprint)
        {
            animator.SetBool(isRunning, false);
        }

    }
}

