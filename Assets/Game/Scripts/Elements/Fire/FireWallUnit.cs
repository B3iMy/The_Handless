using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallUnit : MonoBehaviour
{
	public float damage;

	public void Initialize(float skillDamage)
	{
		damage = skillDamage;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check enemy impact with fire wall unit
		if (collision.gameObject.CompareTag("EnemyHitbox"))
		{
			EnemyBehaviour enemy = collision.GetComponentInParent<EnemyBehaviour>();
			if (enemy != null)
			{
				enemy.TakeHit(damage);
				Destroy(gameObject); // Destroy each unit of fire wall
			}
		}
	}
}