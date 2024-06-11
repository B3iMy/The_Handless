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
		attackCooldown -= Time.deltaTime; // Giảm bộ đếm thời gian theo thời gian thực

		if (attackCooldown <= 0f) // Chỉ tấn công khi bộ đếm thời gian bằng hoặc nhỏ hơn 0
		{
			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
			foreach (Collider2D enemy in hitEnemies)
			{
				enemyHit = enemy.GetComponent<EnemyBehaviour>();

				if (enemyHit != null)
				{
					//enemyHit.TakeHit(entity.atk);
					attackCooldown = attackDelay; // Đặt lại bộ đếm thời gian
					//Debug.Log("Player tấn công Enemy");
				}
			}
		}
	}
}
