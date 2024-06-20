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
<<<<<<< HEAD
        // Add SphereCollider if not already present
        //SphereCollider collider = gameObject.GetComponent<SphereCollider>();
        //if (collider == null)
        //{
        //    collider = gameObject.AddComponent<SphereCollider>();
        //}

        //collider.isTrigger = true; // Set as trigger if you want to use OnTriggerEnter instead of OnCollisionEnter
=======
>>>>>>> Khánh
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