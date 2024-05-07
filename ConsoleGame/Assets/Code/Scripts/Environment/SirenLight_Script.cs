using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SirenLight_Script : MonoBehaviour
{
    public float rotationSpeed = 120f; // Rotation speed in degrees per second
    public bool alarm = false;

    private void Start()
    {
        this.GetComponent<Light>().intensity = 0;
        PlayerPrefs.SetInt("Alarm", 0);
    }
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.P) && alarm)
        {
            AlarmOff();
        }
        else if(Input.GetKeyDown(KeyCode.P) && !alarm || PlayerPrefs.GetInt("Alarm") == 1 && !alarm)
        {
            Debug.Log("Trigger Alarm");
            AlarmOn();
        }
        if (alarm)
        {
            // Rotate the GameObject around its vertical axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }   

    }
    public void AlarmOn()
    {
        this.GetComponent<Light>().intensity = 11;
        PlayerPrefs.SetInt("Alarm", 1);
        PlayerPrefs.Save();
        alarm = !alarm;
        //var sirenAudio = FMODUnity.RuntimeManager.CreateInstance("event:/Joao/Siren");
        //sirenAudio.start();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Joao/Siren");
        GameObject.FindGameObjectWithTag("AIController").GetComponent<Ai_Controller>().EnableSecurityAI();
    }
    public void AlarmOff()
    {
        this.GetComponent<Light>().intensity = 0;
        PlayerPrefs.SetInt("Alarm", 0);
        PlayerPrefs.Save();
        alarm = !alarm;
    }
}
