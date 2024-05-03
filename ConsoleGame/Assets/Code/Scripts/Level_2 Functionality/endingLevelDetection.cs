using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingLevelDetection : MonoBehaviour
{
    
    public endingLevelCamera endinglevelcamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endinglevelcamera.switchCamera();
        }
    }

}
