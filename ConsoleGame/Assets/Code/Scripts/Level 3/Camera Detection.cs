using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{
    // Detect trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is tagged as "Player"
        if (other.gameObject.tag == "Player")
        {
            // Check if the alarm status is not set
            if (PlayerPrefs.GetInt("Alarm") == 0)
            {
                // Set the alarm status to 1
                PlayerPrefs.SetInt("Alarm", 1);
            }
        }
    }
}