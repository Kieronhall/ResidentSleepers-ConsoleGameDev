using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openGateCollider : MonoBehaviour
{
    public string playerTag = "Player";
    public CameraSwitcher cameraSwitcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
                Debug.Log("Player collided with the gate and cameraSwitch triggered");
                cameraSwitcher.switchCamera();
        }
    }
}
