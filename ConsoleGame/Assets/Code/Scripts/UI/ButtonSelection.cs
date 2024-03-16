using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public Button primaryButton;
    void Start()
    {
        primaryButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
