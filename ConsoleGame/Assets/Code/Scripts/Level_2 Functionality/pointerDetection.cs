using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("rhythmHitPoint"))
        {
            Debug.Log("Collision with rhythm hit point detected!");
        }
    }
}
