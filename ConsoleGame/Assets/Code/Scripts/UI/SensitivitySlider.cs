using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField]private ThirdPersonAim cameraSens;
    [SerializeField]private Slider sliderSens;
    [SerializeField]private Slider sliderAimSens;
    

    private const string NormalSensitivityKey = "NormalSensitivity";
    private const string AimSensitivityKey = "AimSensitivity";

    private void Start()
    {
        // Load sensitivity values from PlayerPrefs
        float normalSensitivity = PlayerPrefs.GetFloat(NormalSensitivityKey, cameraSens.normalSensitivity);
        float aimSensitivity = PlayerPrefs.GetFloat(AimSensitivityKey, cameraSens.aimSensitivity);

        // Set slider values
        sliderSens.value = normalSensitivity;
        sliderAimSens.value = aimSensitivity;

        // Add listeners to the slider value change events
        sliderSens.onValueChanged.AddListener(OnSensivityChanged);
        sliderAimSens.onValueChanged.AddListener(OnAimSensitivityChanged);
    }

    public void OnSensivityChanged(float newValue)
    {
        cameraSens.normalSensitivity = newValue;
        PlayerPrefs.SetFloat(NormalSensitivityKey, newValue);
    }

    public void OnAimSensitivityChanged(float newValue)
    {
        cameraSens.aimSensitivity = newValue;
        PlayerPrefs.SetFloat(AimSensitivityKey, newValue);
    }
}
