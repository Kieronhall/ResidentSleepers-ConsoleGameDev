using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType 
    {
        MASTER,
        MUSIC,
        AMBIENCE,
        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioSettings.instance.masterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioSettings.instance.musicVolume;
                break;
            default:
                Debug.LogWarning("Volume type not supported: " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioSettings.instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                AudioSettings.instance.musicVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume type not supported: " + volumeType);
                break;
        }
    }


}
