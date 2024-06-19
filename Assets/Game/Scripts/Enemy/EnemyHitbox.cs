using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
	[SerializeField] private float radius = 1f;
	[SerializeField] private LayerMask playerLayer;

	[SerializeField] private float damageDelay = 1f;
	private float lastDamageTime;

	public GolemBehaviour golemBehaviour; // Reference to GolemBehaviour

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

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
				}
			}
		}
	}
}
