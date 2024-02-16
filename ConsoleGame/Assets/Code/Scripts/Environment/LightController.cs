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
        
    }
}
