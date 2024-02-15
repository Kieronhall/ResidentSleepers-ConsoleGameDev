using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automatically Add Character Controller
[RequireComponent(typeof(CharacterController))]


public class KBM_Controller : MonoBehaviour
{
    //public CharacterController controller;
    //public float speed = 6f;


    //// Update is called once per frame
    //void Update()
    //{

    //}

    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Hide the cursor
        Cursor.visible = false;
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * verticalInput + transform.right * horizontalInput;
        controller.Move(movement * moveSpeed * Time.deltaTime);

        // Player rotation based on camera direction (if applicable)
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, mouseX);
    }
}
