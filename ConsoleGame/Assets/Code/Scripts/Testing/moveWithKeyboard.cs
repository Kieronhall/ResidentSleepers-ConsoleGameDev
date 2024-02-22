using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWithKeyboard : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input values for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Normalize the movement vector to prevent faster diagonal movement
        movement.Normalize();

        // Move the GameObject based on input
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // GetComponent<Rigidbody>().MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }
}
