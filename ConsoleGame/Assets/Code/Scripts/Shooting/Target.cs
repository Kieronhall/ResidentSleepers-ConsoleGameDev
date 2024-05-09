using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health ;
    public float maxHealth = 100f;
    public GameObject extraAmmo;
    private float chanceSpawn = 0.45f;
    private float randomValue;
    //EnemyHealthBar enemyHealthBar;

    Agent agent = null;
    
    private void Start()
    {
        //enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        agent = this.gameObject.GetComponent<Agent>();
    }

    public void Die()
    {
        if (agent != null)
        {
            agent.deathMovement();
            deathAnimation();
            Invoke("DestroyObject", 4f);
            chanceSpawn = 0.45f;
            //enemyHealthBar.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void DestroyObject()
    {
        randomValue = Random.Range(0f, 1f);
        if (randomValue <= chanceSpawn)
        {
            Instantiate(extraAmmo, transform.position, transform.rotation);
        }
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
