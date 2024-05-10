using FMODUnity;

public class Terminal : Interactable
{
    // Reference to the ASyncLoader component and the scene name to load
    public ASyncLoader loader;
    public string sceneName;

    // Method to handle interaction with the terminal
    protected override void Interact()
    {
        // Stop all events on the master bus with fade-out allowed
        var masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        // Load and transition to the next level using the ASyncLoader component
        loader.LoadLevelButton(sceneName);
    }
}