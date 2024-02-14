using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenLight_Script : MonoBehaviour
{
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the GameObject around its vertical axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
