using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Prompt message to display when interacting with the object
    [SerializeField] public string promptMessage;

    // Method to trigger interaction with the object
    public void BaseInteract()
    {
        Interact();
    }

    // Virtual method to handle interaction (to be overridden by derived classes)
    protected virtual void Interact()
    {
        // Implementation left empty for specific interactions in derived classes
    }
}