using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerLvl1 : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text uiText;

    void Start()
    {
        uiText.text = "Infiltrate the Facility! Watchout for the Guards!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DisableExterior"))
        {
            Debug.Log("Exterior Disabled");
            GameObject.FindGameObjectWithTag("Exterior").SetActive(false);
        }
        if (other.name=="Zone1")
        {
            Debug.Log("Zone1");
            uiText.text = "Avoid being detected and locate the backdoor! Hide in the bins if you need!";
        }
        if (other.name == "Zone2")
        {
            Debug.Log("Zone2");
            uiText.text = "The Backdoor is Locked! Locate the Eletric Panel!";
        }
        if (other.name == "Zone3")
        {
            Debug.Log("Zone3");
            uiText.text = "Navigate the Ground Floor until you find the Laser Room!";
        }
        if (other.name == "Zone4")
        {
            Debug.Log("Zone4");
            uiText.text = "The Laser Room! Avoid being hit and make it to the staircase door!";
        }
        if (other.name == "Zone5")
        {
            Debug.Log("Zone5");
            uiText.text = "Get to the Top Floor and Disable Lockdown Mode!";
        }
        if (other.name == "Zone6")
        {
            Debug.Log("Zone6");
            uiText.text = "Locate and Disable Lockdown! Also Find the Lift to the Cell Floors!";
        }

    }
}
