using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.Rendering;

public class DestroyTripWire : MonoBehaviour
{
    public Door_Script doorLock;
    public PlayerControls playerControls;
    public GameObject traps;
    public CameraSwitcher cameraSwitch;
    private void OnTriggerStay(Collider other)
    {
        //cameraSwitch.switchCamera();
        //doorLock.LockDoor(1);
        //Destroy(traps);
        if (playerControls.interact)
        {
            cameraSwitch.switchCamera();
            doorLock.LockDoor(1);
            Destroy(traps);
        }
    }
}
