using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public ScriptableEntity entity;

    public float Hitpoints;
    public float MaxHitpoints;
    public HealthBarBehaviour Healthbar;
    void Start()
    {
        MaxHitpoints = entity.hp;
        Hitpoints = MaxHitpoints;
        Healthbar.SetHealth(Hitpoints, MaxHitpoints);
    }
    public void TakeHit(float damage)
    {
        Hitpoints -= damage;

        if (Hitpoints <= 0)
        {
            Destroy(gameObject);
        }
        // Update the health bar
        Healthbar.SetHealth(Hitpoints, MaxHitpoints);
        Healthbar.gameObject.SetActive(true);
    }
}