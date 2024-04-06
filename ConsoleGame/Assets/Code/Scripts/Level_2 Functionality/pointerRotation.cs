using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerRotation : MonoBehaviour
{
    public float pointerRotationSpeed = 30f; 

    private void Update()
    {
       transform.Rotate(Vector3.left, pointerRotationSpeed * Time.deltaTime);
    }
}
