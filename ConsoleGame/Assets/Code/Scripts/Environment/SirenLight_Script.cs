using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenLight_Script : MonoBehaviour
{
    public float rotationSpeed = 120f; // Rotation speed in degrees per second
    public bool alarm = false;

    private void Start()
    {
        this.GetComponent<Light>().intensity = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && alarm)
        {
            this.GetComponent<Light>().intensity=0;
            alarm = !alarm;
        }
        else if(Input.GetKeyDown(KeyCode.P) && !alarm)
        {
            this.GetComponent<Light>().intensity = 11;
            alarm = !alarm;
        }
        if (alarm)
        {
            // Rotate the GameObject around its vertical axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }   
    }
}
