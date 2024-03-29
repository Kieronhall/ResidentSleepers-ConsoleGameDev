using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class takeDamage : MonoBehaviour
{
    public enum collisionType { head, body}
    public collisionType damageType;

    public Target target;

    public void Hit(float damage)
    {
        target.health -= damage;
        if (target.health <= 0)
        {
            target.Die();
        }
    }
}
