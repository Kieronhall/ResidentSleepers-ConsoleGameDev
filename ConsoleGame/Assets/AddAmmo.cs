using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : MonoBehaviour
{
    public int addBullets = 6;
    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Pistol_Player pistol_Player = other.GetComponent<Pistol_Player>();
        pistol_Player.AddAmmo(addBullets);
        Destroy(gameObject);
    }

}
