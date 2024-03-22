//using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : MonoBehaviour
{

    public int addBullets = 6;
    private void OnTriggerEnter(Collider collision)
    {
        Pistol_Player pistol_Player = collision.GetComponent<Pistol_Player>();
        if (pistol_Player != null)
        {
            pistol_Player.AddAmmo(addBullets);
            Destroy(gameObject);
        }
    }
}
