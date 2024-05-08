using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerRotation : MonoBehaviour
{
    public float pointerRotationSpeed = 30f;
    private bool rotateLeft = true;

    private void Update()
    {
        if (rotateLeft)
        {
            transform.Rotate(Vector3.left, pointerRotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.right, pointerRotationSpeed * Time.deltaTime);
        }
    }

    public void SwitchRotation()
    {
        rotateLeft = !rotateLeft;
    }
}
