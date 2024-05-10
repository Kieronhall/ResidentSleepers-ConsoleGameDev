using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Tracking_Script : MonoBehaviour
{

    public float targetRotation = 30f; // Target rotation in degrees
    public float rotationDuration = 8f; // Duration of rotation in seconds

    private Quaternion targetQuaternion;
    private Quaternion initialRotation;
    private float elapsedTime = 0f;

    private void Start()
    {
        // Store the initial rotation of the GameObject
        initialRotation = transform.rotation;

        // Calculate the target quaternion based on the target rotation
        targetQuaternion = Quaternion.Euler(targetRotation, 0f, 0f) /* initialRotation*/;
    }

    private void Update()
    {
        //transform.Rotate(new Vector3(30, 0, 0), rotationDuration * Time.deltaTime);

        transform.rotation = Quaternion.Euler(Vector3.forward * 30f);
    }
}
