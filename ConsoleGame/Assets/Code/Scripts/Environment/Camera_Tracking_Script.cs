using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Tracking_Script : MonoBehaviour
{
    //public float distance = 10f; // Public variable for distance
    //public float detectionAngle = 80f; // Angle in degrees for the detection area
    //private Transform playerTransform;
    //sensors S;
    //private void Start()
    //{
    //    S = this.gameObject.GetComponent<sensors>();
    //    // Find the player GameObject (assuming it has a tag "Player")
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    if (player != null)
    //    {
    //        // Get the player's transform component
    //        playerTransform = player.transform;
    //    }
    //    else
    //    {
    //        Debug.LogError("Player not found!");
    //    }
    //}

    //private void Update()
    //{
    //    // Check if the player transform is assigned
    //    if (playerTransform != null && S.Hit==true)
    //    {
    //        // Calculate the direction to the player
    //        Vector3 directionToPlayer = playerTransform.position - transform.position;

    //        // Check if the player is within the specified distance and within the detection angle
    //        if (directionToPlayer.magnitude <= distance && Vector3.Angle(transform.forward, directionToPlayer) <= detectionAngle / 2f)
    //        {
    //            // Face the player (assuming forward is the Z-axis)
    //            transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    //        }
    //    }
    //}

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
