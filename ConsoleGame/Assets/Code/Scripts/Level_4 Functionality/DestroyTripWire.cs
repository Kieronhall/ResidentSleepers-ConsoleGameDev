using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;

public class DestroyTripWire : MonoBehaviour
{
    public PlayerControls playerControls;
    public GameObject traps;
    private void OnTriggerStay(Collider other)
    {
        if (playerControls.interact)
        {
            Destroy(traps);
        }
    }
}
