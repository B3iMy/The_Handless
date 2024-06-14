using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public ScriptableEntity entity;

	[SerializeField] protected float hitpoints;
	[SerializeField] protected float maxHitpoints;
	[SerializeField] protected HealthBarBehaviour healthbar;

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

	public void TakeHit(float damage)
	{
		hitpoints -= damage;

		if (hitpoints <= 0)
		{
			hitpoints = 0;
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
