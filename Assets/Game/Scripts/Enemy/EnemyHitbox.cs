using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	[SerializeField] private float radius = 1f;
	[SerializeField] private LayerMask playerLayer; // Player layer

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	private void Update()
	{
		Collider2D[] playerHit = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
		foreach (Collider2D enemy in playerHit)
		{
			Debug.Log("Player mất máu");
		}
	}
}
