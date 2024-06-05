using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageButton : MonoBehaviour
{
    public ScriptableEntity entity;
    public EnemyBehaviour enemy;

    public void DamageEnemy()
    {
        enemy.TakeHit(entity.atk); // Use player's attack value
        Debug.Log("This button is clicked and Player attacks with " + entity.atk + " damage!");
        //enemy.TakeHit(1); // Use player's attack value
        //Debug.Log("This button is clicked and Player attacks with " + 1 + " damage!");
    }
}
