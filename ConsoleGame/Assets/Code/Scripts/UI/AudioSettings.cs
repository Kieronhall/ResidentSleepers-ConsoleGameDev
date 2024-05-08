using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;


public class AudioSettings : MonoBehaviour
{

    [field:Header("Volume")]
    [Range(0, 1)]

    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]

    private Bus masterBus;
    private Bus musicBus;

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }


    public static AudioSettings instance { get; private set; }

    [SerializeField]
    private Slider musicSlider;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("found more thant one audio manager");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");

    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
    }
}
