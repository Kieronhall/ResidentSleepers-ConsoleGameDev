using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light targetLight; // Reference to the light component you want to change

    private void Start()
    {
        targetLight = this.GetComponent<Light>();
    }
    void Update()
    {
        // Check if the F key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Change the color of the light to red
            if (targetLight != null)
            {
                targetLight.color = Color.red;
            }
            else
            {
                Debug.LogWarning("Target light is not assigned!");
            }
        }
    }
}
