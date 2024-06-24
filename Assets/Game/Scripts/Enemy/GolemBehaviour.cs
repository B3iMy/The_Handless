using UnityEngine;

public class GolemBehaviour : MonoBehaviour
{
	public Golem stats;

	private float skillCooldownTimer;
	private float skillDurationTimer;
	public bool isUsingSkill;

	private Animator animator;
	private Rigidbody2D rb;
	private EnemyMovement enemyMovement;
	private Transform playerTransform;

	private Vector3 originalScale; // To store the original scale

	// Laser setting
	[SerializeField] private GameObject laserPrefab;
	[SerializeField] private Transform firePoint; // the point where the laser is fired from
	[SerializeField] private float laserSpeed = 10f;
	[SerializeField] private float laserCooldown = 5f;
	[SerializeField] private float laserSpreadAngle = 30f; // angle between lasers
	private float lastLaserTime;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		enemyMovement = GetComponent<EnemyMovement>();

		if (enemyMovement == null)
		{
			Debug.LogError("EnemyMovement component not found on " + gameObject.name);
		}

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		if (playerTransform == null)
		{
			Debug.LogError("Player not found in the scene.");
		}

		originalScale = transform.localScale; // Store the original scale
	}

	private void Start()
	{
		skillCooldownTimer = Random.Range(1f, stats.whirlwind_cooldown);
		lastLaserTime = -laserCooldown; // Initialize so that laser can be fired immediately if needed
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

		// Fire laser if cooldown is over
		if (Time.time - lastLaserTime >= laserCooldown && !isUsingSkill)
		{
			FireLaser();
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
		enemyMovement.enabled = false;
	}

	private void EndWhirlwind()
	{
		isUsingSkill = false;

		// Stop whirlwind animation and behavior
		animator.SetTrigger("WhirlwindEnd");

		// Resume normal behavior
		enemyMovement.enabled = true;
	}

	private void WhirlwindMove()
	{
		if (playerTransform != null)
		{
			// Calculate direction towards player
			Vector2 direction = (playerTransform.position - transform.position).normalized;
			rb.velocity = direction * stats.whirlwindSpeed;

			// Flip the GameObject based on movement direction
			Flip(direction);
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

	private void FireLaser()
	{
		if (firePoint != null && laserPrefab != null)
		{
			// calculate the firing direction of the laser
			Vector2 directionToPlayer = (playerTransform.position - firePoint.position).normalized;
			float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

			// shoot laser in three different angle
			FireSingleLaser(baseAngle);
			FireSingleLaser(baseAngle + laserSpreadAngle);
			FireSingleLaser(baseAngle - laserSpreadAngle);

			lastLaserTime = Time.time;
		}
		else
		{
			Debug.LogError("Fire point or laser prefab is not set.");
		}
	}

	private void FireSingleLaser(float angle)
	{
		GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

		// Set the velocity of the laser
		Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
		if (laserRb != null)
		{
			laserRb.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * laserSpeed;
		}
	}

	private void Flip(Vector2 direction)
	{
		if (direction.x < 0 && transform.localScale.x > 0)
		{
			transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
		}
		else if (direction.x > 0 && transform.localScale.x < 0)
		{
			transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
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
