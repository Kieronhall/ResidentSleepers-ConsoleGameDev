using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public Button primaryButton;
    void Start()
    {
        primaryButton.Select();
    }
}
