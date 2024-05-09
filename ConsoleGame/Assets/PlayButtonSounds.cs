using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;
using UnityEditor.Localization.Plugins.XLIFF.V12;

public class PlayButtonSounds : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Sound button on select")]
    public EventReference soundEffect ;
    [Header("Sound button on click")]
    public EventReference soundOnClick;
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        // Check if the event is triggered by a gamepad
        if (eventData is AxisEventData)
        {
            PlayOnSelectSound();
        }
    }
    public void PlayOnSelectSound()
    {
        RuntimeManager.PlayOneShot(soundEffect);
    }
    public void PlayOnClick()
    {
        RuntimeManager.PlayOneShot(soundOnClick);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // You can add logic here if you want to handle deselection events
    }
}
