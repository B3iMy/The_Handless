using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	[SerializeField] private float radius = 1f;
	[SerializeField] private LayerMask playerLayer; // Player layer

	[SerializeField] private float attackDelay = 0.5f;

	public PlayerBehaviour playerHit;
	public ScriptableEntity entity;

	[SerializeField] private float attackCooldown = 0f;

	private Animator animator;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	private void Awake()
	{
		animator = GetComponentInParent<Animator>(); // Get the Animator from the parent
	}

	private void Update()
	{
		attackCooldown -= Time.deltaTime;

		if (attackCooldown <= 0f)
		{
			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
			foreach (Collider2D enemy in hitEnemies)
			{
				playerHit = enemy.GetComponent<PlayerBehaviour>();

				if (playerHit != null)
				{
					animator.SetTrigger("Attack");
					playerHit.TakeHit(entity.atk);
					attackCooldown = attackDelay;
				}
			}
		}
	}
}
