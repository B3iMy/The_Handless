using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    public float Hitpoints;
    public float MaxHitpoints = 10;
    public HealthBarBehaviour Healthbar;

    void Start()
    {
        Hitpoints = MaxHitpoints;
        if (Healthbar != null)
        {
            Healthbar.SetHealth(Hitpoints, MaxHitpoints);
        }
    }

    public void TakeHit(float damage)
    {
        Hitpoints -= damage;

        if (Healthbar != null)
        {
            Healthbar.SetHealth(Hitpoints, MaxHitpoints);
        }

        if (Hitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
