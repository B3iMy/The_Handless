using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public ScriptableEntity entity;

	public float Hitpoints;
	public float MaxHitpoints;
	public HealthBarBehaviour Healthbar;

	void Start()
	{
		if (entity != null)
		{
			MaxHitpoints = entity.hp;
			Hitpoints = MaxHitpoints;
			UpdateHealthBar();
		}
		else
		{
			Debug.LogError("ScriptableEntity is not assigned!");
		}
	}

	public void TakeHit(float damage)
	{
		Hitpoints -= damage;

		if (Hitpoints <= 0)
		{
			Hitpoints = 0;
			Destroy(gameObject);
		}

		UpdateHealthBar();
	}

	private void UpdateHealthBar()
	{
		if (Healthbar != null)
		{
			Healthbar.SetHealth(Hitpoints, MaxHitpoints);
			Healthbar.gameObject.SetActive(true);
		}
		else
		{
			Debug.LogError("Healthbar is not assigned!");
		}
	}
}