using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingLevelCamera : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    private bool isCamera1Active = true;

    public bool gameComplete = false;


    void Start()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }

    void Update()
    {
 
    }

    public void switchCamera()
    {
        camera1.enabled = false;
        camera2.enabled = true;
    }

    public void switchCameraBack()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }
}
