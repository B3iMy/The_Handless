using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class BulletSpawn : MonoBehaviour
{
	enum SpawnerType { Straight, Spin }

	[Header("Bullet Attributes")]
	public GameObject bullet;
	public float bulletLife = 1f;
	public float speed = 1f;

	[Header("Spawner Attributes")]
	[SerializeField] private SpawnerType spawnerType;
	[SerializeField] private float firingRate = 1f;

	[Header("References")]
	public GolemBehaviour golemBehaviour;

	[Header("Delay Settings")]
	[SerializeField] private float activationDelay = 2f; // Delay between activations
	[SerializeField] private float activeDuration = 3f; // Duration of spawner being active

	private GameObject spawnedBullet;
	private float timer = 0f;
	private float activationTimer = 0f; // Timer for activation delay
	private float activeTimer = 0f; // Timer for active duration
	private bool isActive = false; // Track if the spawner is currently active

	void Start()
	{
		activationTimer = activationDelay; // Initialize activation timer
		activeTimer = activeDuration; // Initialize active timer
	}

	void Update()
	{
		if (isActive)
		{
			// Active state: handle bullet firing and active timer
			activeTimer -= Time.deltaTime;

			timer += Time.deltaTime;
			if (spawnerType == SpawnerType.Spin)
				transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 1f);

			if (timer >= firingRate)
			{
				Fire();
				timer = 0;
				if (golemBehaviour != null)
				{
					golemBehaviour.SetIdle(true);
				}
			}

			// Check if active duration is over
			if (activeTimer <= 0)
			{
				isActive = false;
				activationTimer = activationDelay; // Reset activation timer after active period
			}
		}
		else
		{
			// Inactive state: handle activation delay timer
			activationTimer -= Time.deltaTime;

			if (activationTimer <= 0)
			{
				isActive = true;
				activeTimer = activeDuration; // Reset active timer when becoming active
			}
		}
	}

	private void Fire()
	{
		if (bullet)
		{
			spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
			spawnedBullet.GetComponent<Bullet>().speed = speed;
			spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
			spawnedBullet.transform.rotation = transform.rotation;
		}
	}
}