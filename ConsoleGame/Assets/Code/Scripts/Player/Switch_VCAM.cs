using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVCAM : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;
    private GameObject player;
    private PlayerController playerController;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        playerController.playerSpeed = 2.0f;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
        Debug.Log("Aiming");
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        playerController.playerSpeed = 4.0f;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
