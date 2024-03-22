using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class RoomDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Floor0":
                Debug.Log("Player In " + other.tag);
                break;
            case "Floor1":
                Debug.Log("Player In " + other.tag);
                break;
            case "Floor2":
                Debug.Log("Player In " + other.tag);
                break;
            case "Floor3":
                Debug.Log("Player In " + other.tag);
                break;
        }
    }
}
