using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    private float maxHealth = 100f;
    EnemyHealthBar enemyHealthBar;
    [SerializeField]
    private GameObject extraAmmo;


    private void Start()
    {
        health = maxHealth;

        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        enemyHealthBar.HealthBarPercentage(health / maxHealth);

        if (health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Destroy(gameObject);
        SpawnAmmo(transform.position);
        enemyHealthBar.gameObject.SetActive(false);

    }

    void SpawnAmmo( Vector3 position)
    {
        Instantiate(extraAmmo, position, Quaternion.identity);
    }
}
