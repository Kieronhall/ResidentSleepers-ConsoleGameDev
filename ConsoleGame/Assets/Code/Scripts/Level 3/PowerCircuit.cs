using UnityEngine;

public class PowerCircuit : Interactable
{
    // References to tripwire game objects and the door
    public GameObject tripWire1;
    public GameObject tripWire2;
    public GameObject tripWire3;
    public GameObject tripWire4;
    public GameObject door;

    // Method to handle interaction with the power circuit
    protected override void Interact()
    {
        // Play sound effect for disabling the power circuit
        FMODUnity.RuntimeManager.PlayOneShot("event:/Dylan/Level 3/Power Cirucit Disable", this.transform.position);

        // Disable tripwires and unlock the door
        DisableTripwire(tripWire1);
        DisableTripwire(tripWire2);
        DisableTripwire(tripWire3);
        DisableTripwire(tripWire4);
        door.GetComponent<Door_Script>().doorLocked = false;
    }

    // Method to disable a tripwire
    private void DisableTripwire(GameObject tripWire)
    {
        // Disable tripwire script and line renderer
        tripWire.GetComponentInChildren<Tripwire_Script>().enabled = false;
        tripWire.GetComponentInChildren<LineRenderer>().enabled = false;
    }
}