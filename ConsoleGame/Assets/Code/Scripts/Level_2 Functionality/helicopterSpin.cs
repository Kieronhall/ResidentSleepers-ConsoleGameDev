using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helicopterSpin : MonoBehaviour
{
    public float rotationSpeed = 90.0f; 

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
