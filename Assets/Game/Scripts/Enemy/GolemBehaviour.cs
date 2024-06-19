using UnityEngine;

public class GolemBehaviour : MonoBehaviour
{
	public Golem stats;

	private float skillCooldownTimer;
	private float skillDurationTimer;
	public bool isUsingSkill;

	private Animator animator;
	private Rigidbody2D rb;
	private EnemyAi enemyAi;

	private Transform playerTransform;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		enemyAi = GetComponent<EnemyAi>();

		if (enemyAi == null)
		{
			Debug.LogError("EnemyAi component not found on " + gameObject.name);
		}

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		if (playerTransform == null)
		{
			Debug.LogError("Player not found in the scene.");
		}
	}

	private void Start()
	{
		skillCooldownTimer = Random.Range(1f, stats.whirlwind_cooldown);
	}

	private void Update()
	{
		if (skillCooldownTimer <= 0 && !isUsingSkill)
		{
			StartWhirlwind();
		}
		else
		{
			skillCooldownTimer -= Time.deltaTime;
		}

		if (isUsingSkill)
		{
			skillDurationTimer -= Time.deltaTime;
			if (skillDurationTimer <= 0)
			{
				EndWhirlwind();
			}
			else
			{
				WhirlwindMove();
			}
		}
	}

	private void StartWhirlwind()
	{
		isUsingSkill = true;
		skillDurationTimer = stats.whirlwind_duration;
		skillCooldownTimer = Random.Range(3f, stats.whirlwind_cooldown); // Set random cooldown for next use

		// Trigger whirlwind animation and behavior
		animator.SetTrigger("Whirlwind");

		// Stop normal movement while using the skill
		rb.velocity = Vector2.zero;
		enemyAi.enabled = false;
	}

	private void EndWhirlwind()
	{
		isUsingSkill = false;

		// Stop whirlwind animation and behavior
		animator.SetTrigger("WhirlwindEnd");

		// Resume normal behavior
		enemyAi.enabled = true;
	}

	private void WhirlwindMove()
	{
		if (playerTransform != null)
		{
			// Calculate direction towards player
			Vector2 direction = (playerTransform.position - transform.position).normalized;
			rb.velocity = direction * stats.whirlwindSpeed;
		}
	}

	// This method could be called from an animation event or another script
	public void ApplyWhirlwindDamage()
	{
		// Implement damage logic
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, stats.range_attack, LayerMask.GetMask("Player"));
		foreach (Collider2D enemy in hitEnemies)
		{
			PlayerBehaviour player = enemy.GetComponent<PlayerBehaviour>();
			if (player != null)
			{
				player.TakeHit(stats.atk);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (stats != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, stats.range_attack);
		}
	}
}
