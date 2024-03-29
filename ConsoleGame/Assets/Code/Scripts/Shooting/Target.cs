using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;
    [SerializeField]
    private GameObject extraAmmo;


    private void Start()
    {
        health = maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
        SpawnAmmo(transform.position);
    }

    void SpawnAmmo( Vector3 position)
    {
        Instantiate(extraAmmo, position, Quaternion.identity);
    }
}
