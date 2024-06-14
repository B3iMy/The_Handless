using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
	[SerializeField] private float radius = 1f;
	[SerializeField] private LayerMask enemyLayer; // Enemy's layer

	[SerializeField] private float attackDelay = 0.5f;

	public EnemyBehaviour enemyHit;
	public ScriptableEntity entity;

	[SerializeField] private float attackCooldown = 0f;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	private void Update()
	{
		attackCooldown -= Time.deltaTime;

		if (attackCooldown <= 0f)
		{
			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
			foreach (Collider2D enemy in hitEnemies)
			{
				enemyHit = enemy.GetComponent<EnemyBehaviour>();

				if (enemyHit != null)
				{
					attackCooldown = attackDelay;
				}
			}
		}
	}
}
