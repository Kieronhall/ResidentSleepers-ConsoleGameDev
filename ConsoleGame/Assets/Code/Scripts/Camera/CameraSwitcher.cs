using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public float switchDuration = 5f;

    private bool isCamera1Active = true;


    void Start()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    StartCoroutine(SwitchCamerasRoutine());
        //}

    }

    IEnumerator SwitchCamerasRoutine()
    {
        camera1.enabled = false;
        camera2.enabled = true;

        yield return new WaitForSeconds(switchDuration);

        camera1.enabled = true;
        camera2.enabled = false;
    }

    public void switchCamera()
    {
        StartCoroutine(SwitchCamerasRoutine());
    }
}
