using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class takeDamage : MonoBehaviour
{
    public enum collisionType { head, body}
    public collisionType damageType;
    private EnemyHealthBar enemyHealthBar;


    public Target target;


    private void Start()
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }
    public void Hit(float damage)
    {
        target.health -= damage;
        //enemyHealthBar.HealthBarPercentage(target.health / target.maxHealth);
        if (target.health <= 0)
        {
            target.Die();
            //enemyHealthBar.gameObject.SetActive(false);
        }
    }
}
