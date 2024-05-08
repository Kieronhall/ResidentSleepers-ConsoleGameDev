using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPrompt : MonoBehaviour
{
    public GameObject ButtonPrompt;
    private PlayerControls playerControls;
    private void Start()
    {
        ButtonPrompt.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ButtonPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ButtonPrompt.SetActive(false);
        }
    }
}
