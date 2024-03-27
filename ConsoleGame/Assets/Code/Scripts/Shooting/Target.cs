using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    private float maxHealth = 100f;
    EnemyHealthBar enemyHealthBar;

    Agent agent;
    
    private void Start()
    {
        health = maxHealth;

        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();

        agent = this.gameObject.GetComponent<Agent>();
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
        agent.deathMovement();
        deathAnimation();
        Invoke("DestroyObject", 4f);
        enemyHealthBar.gameObject.SetActive(false);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void deathAnimation()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isDead", true);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isRunning", false);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isWalking", false);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isShooting", false);
    }
}
