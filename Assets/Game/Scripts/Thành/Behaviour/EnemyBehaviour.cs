using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public ScriptableEntity entity;

	[SerializeField] protected float hitpoints;
	[SerializeField] protected float maxHitpoints;
	[SerializeField] protected HealthBarBehaviour healthbar;

<<<<<<< Updated upstream
	private void Start()
=======
	public event Action OnEnemyKilled;

    // Reference to the DamageTracker
	#if UNITY_EDITOR
    public DamageTracker damageTracker;
	#endif

    private void Start()
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
=======
		string playerName = player.tag; // Use tag to identify player

		Debug.Log($"Boss take {damage} damage from {playerName}");

        // Add damage to DamageTracker
	#if UNITY_EDITOR

        if (damageTracker != null)
		{
			damageTracker.AddDamage(playerName, damage);
		}
	#endif

>>>>>>> Stashed changes
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
