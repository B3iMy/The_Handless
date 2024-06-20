<<<<<<< HEAD
﻿using System.Collections;
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
=======
﻿using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
	[SerializeField] private float radius = 1f;
	[SerializeField] private LayerMask playerLayer;

	[SerializeField] private float damageDelay = 1f;
	private float lastDamageTime;

	public GolemBehaviour golemBehaviour; // Reference to GolemBehaviour
>>>>>>> Khánh

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

<<<<<<< HEAD
	private void Update()
	{
		attackCooldown -= Time.deltaTime; // Giảm bộ đếm thời gian theo thời gian thực

		if (attackCooldown <= 0f) // Chỉ tấn công khi bộ đếm thời gian bằng hoặc nhỏ hơn 0
		{
			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
			foreach (Collider2D enemy in hitEnemies)
			{
				playerHit = enemy.GetComponent<PlayerBehaviour>();

				if (playerHit != null)
				{
					playerHit.TakeHit(entity.atk);
					attackCooldown = attackDelay; // Đặt lại bộ đếm thời gian
					Debug.Log("Player tấn công Enemy");
=======
	private void Awake()
	{
		golemBehaviour = GetComponentInParent<GolemBehaviour>();
	}

	private void Update()
	{
		// Check if Golem is using Whirlwind
		if (golemBehaviour != null && golemBehaviour.isUsingSkill)
		{
			if (Time.time - lastDamageTime > damageDelay)
			{
				// Detect players within the radius
				Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
				foreach (Collider2D playerCollider in hitPlayers)
				{
					// Ensure the player is within the damage range of the skill
					if (Vector2.Distance(playerCollider.transform.position, transform.position) <= radius)
					{
						// Apply damage via GolemBehaviour's ApplyWhirlwindDamage method
						golemBehaviour.ApplyWhirlwindDamage();
						lastDamageTime = Time.time;
					}
>>>>>>> Khánh
				}
			}
		}
	}
}
