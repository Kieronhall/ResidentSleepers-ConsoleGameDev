using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public bool aimDownSights;

    public Transform comabatLookAt;

    public GameObject thirdPersonCam;
    public GameObject comabtCam;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Default,
        Comabt
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SwitchCameraStyle(CameraStyle.Comabt);
            aimDownSights = true;
        }
        else if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            SwitchCameraStyle(CameraStyle.Default);
            aimDownSights = false;
        }

        // Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate player object
        if (currentStyle == CameraStyle.Default)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (currentStyle == CameraStyle.Comabt)
        {
            Vector3 dirToCombatLookAt = comabatLookAt.position - new Vector3(transform.position.x, comabatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        comabtCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if (newStyle == CameraStyle.Default)
        {
            thirdPersonCam.SetActive(true);
        }
        if (newStyle == CameraStyle.Comabt)
        {
            comabtCam.SetActive(true);
        }

        currentStyle = newStyle;
    }
}
