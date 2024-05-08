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
    }
}
