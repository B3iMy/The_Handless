using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
	[SerializeField] private float radius = 1f;
	[SerializeField] private LayerMask enemyLayer; // Enemy's layer

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	private void Update()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
		foreach (Collider2D enemy in hitEnemies)
		{
			Debug.Log("Player tấn công Enemy");
		}
	}
}
