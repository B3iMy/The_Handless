using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public Golem entity;

	[SerializeField] protected float hitpoints;
	[SerializeField] protected float maxHitpoints;
	[SerializeField] protected HealthBarBehaviour healthbar;

	public event Action OnEnemyKilled;

	// Reference to the DamageTracker
	public DamageTracker damageTracker;

	private void Start()
	{
		InitializeEnemy();
	}

	private void InitializeEnemy()
	{
		if (entity != null)
		{
			maxHitpoints = entity.hp;
			hitpoints = maxHitpoints;
			healthbar = GetComponentInChildren<HealthBarBehaviour>();

			if (healthbar != null)
			{
				healthbar.SetHealth(hitpoints, maxHitpoints);
				healthbar.gameObject.SetActive(true);
			}
			else
			{
				Debug.LogError("Healthbar component not found on child GameObject!");
			}
		}
		else
		{
			Debug.LogError("ScriptableEntity is not assigned!");
		}
	}

	public void TakeHit(float damage, GameObject player)
	{
		hitpoints -= damage;

		string playerName = player.tag; // Use tag to identify player

		Debug.Log($"Boss take {damage} damage from {playerName}");

		// Add damage to DamageTracker
		if (damageTracker != null)
		{
			damageTracker.AddDamage(playerName, damage);
		}

		if (hitpoints <= 0)
		{
			hitpoints = 0;
			OnEnemyKilled.Invoke();
			Destroy(gameObject);
		}

		UpdateHealthBar();
	}

	private void UpdateHealthBar()
	{
		if (healthbar != null)
		{
			healthbar.SetHealth(hitpoints, maxHitpoints);
			healthbar.gameObject.SetActive(true);
		}
		else
		{
			Debug.LogError("Healthbar component not found on child GameObject!");
		}
	}
}
