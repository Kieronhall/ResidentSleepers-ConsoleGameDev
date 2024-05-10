using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Prompt : MonoBehaviour
{
    public Canvas canvasPrompt;
    // Start is called before the first frame update
    void Start()
    {
        canvasPrompt.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canvasPrompt.gameObject.SetActive(true); // Set the canvas to be visible
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasPrompt.gameObject.SetActive(false); // Set the canvas to be visible
        }
    }
}
