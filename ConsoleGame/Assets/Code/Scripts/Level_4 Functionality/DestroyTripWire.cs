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
    private FMOD.Studio.EventInstance ElectricDischarge;

    private void OnTriggerStay(Collider other)
    {
        if (playerControls.interact)
        {
            ElectricDischarge = FMODUnity.RuntimeManager.CreateInstance("event:/Joao/ElectricPanel");
            ElectricDischarge.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            ElectricDischarge.start();
            ElectricDischarge.release();
            cameraSwitch.switchCamera();
            doorLock.LockDoor(1);
            Destroy(traps, 1);
        }
    }
}
