using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBarParent;
    public Image healthBar;
    public float healthCurrentAmount;
    public float healthMaxAmount = 100f;

    public void Start()
    {
        healthCurrentAmount = healthMaxAmount;
        healthBarParent.SetActive(false);
    }
    public void Update()
    {
        if (healthCurrentAmount < healthMaxAmount) 
        {
            healthBarParent.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamge(20);
        }

    }
    public void TakeDamge(float damage)
    {
        healthCurrentAmount -= damage;
        healthBar.fillAmount = Mathf.Min(healthCurrentAmount, 0f);
        UpdateHealthBar();

        if (healthCurrentAmount <= 0)
        {
            // Hide the health bar if health drops to or below 0
            healthBarParent.SetActive(false);
        }
    }
    void UpdateHealthBar()
    {
        healthBar.fillAmount = healthCurrentAmount / healthMaxAmount;
    }

}
