using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openGate : MonoBehaviour
{
    public string playerTag = "Player";
    public CameraSwitcher cameraSwitcher;
    public float delayBeforeCameraSwitch = 5f;
    private bool hasSwitchedCamera = false;
    public rhythmGameManager rhythmgamemanager;

    //Gate Opening
    public Transform objectToRotate; 
    public float targetAngle = -52.885f; 
    public float duration = 1.0f; 
    private float startTime;
    private Quaternion startRotation;
    private bool isRotating = false;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag(playerTag))
        {
            Debug.Log("Player collided with the gate and cameraSwitch triggered");
            StartCoroutine(DelayedSwitchCamera());
            StartRotation();

            rhythmgamemanager.rhythmGameActive();
            hasTriggered = true;
        }
    }

    void Update()
    {
        if (isRotating)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            objectToRotate.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(0, targetAngle, 0), t);

            if (t >= 1.0f)
            {
                isRotating = false;
            }
        }
    }

    void StartRotation()
    {
        startTime = Time.time;
        startRotation = objectToRotate.rotation;
        isRotating = true;
    }

    private IEnumerator DelayedSwitchCamera()
    {
        if (!hasSwitchedCamera)
        {
            hasSwitchedCamera = true; 

            yield return new WaitForSeconds(delayBeforeCameraSwitch);
            PlayerPrefs.SetInt("Alarm", 1);
            cameraSwitcher.switchCamera();
        }
    }
}
