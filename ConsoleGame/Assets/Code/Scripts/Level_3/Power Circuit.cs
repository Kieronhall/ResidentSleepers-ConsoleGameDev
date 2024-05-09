using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCircuit : Interactable
{
    public GameObject tripWire1;
    public GameObject tripWire2;
    public GameObject tripWire3;
    public GameObject tripWire4;
    public GameObject door;
    protected override void Interact()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Dylan/Level 3/Power Cirucit Disable", this.transform.position);
        DisableTripwire(tripWire1);
        DisableTripwire(tripWire2);
        DisableTripwire(tripWire3);
        DisableTripwire(tripWire4);
        door.GetComponent<Door_Script>().doorLocked = false;
    }

    private void DisableTripwire(GameObject tripWire)
    {
        tripWire.GetComponentInChildren<Tripwire_Script>().enabled = false;
        tripWire.GetComponentInChildren<LineRenderer>().enabled = false;
    }
}