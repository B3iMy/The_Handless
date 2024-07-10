using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed = 10f;
	[SerializeField] private float damage = 10f;
	[SerializeField] private float lifetime = 3f;
	private Vector2 direction;

	public GameObject owner;

	private void Start()
	{
		Destroy(gameObject, lifetime);
	}

	public void Initialize(Vector2 dir, float projSpeed, float projDamage)
	{
		direction = dir.normalized;
		speed = projSpeed;
		damage = projDamage;
	}

	private void Update()
	{
		transform.Translate(direction * speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check impact to hitbox with tag "EnemyHitbox"
		if (collision.gameObject.CompareTag("EnemyHitbox"))
		{
			EnemyBehaviour enemy = collision.GetComponentInParent<EnemyBehaviour>();
			if (enemy != null)
			{
				// Assuming the player's name is the root GameObject's name
				string playerName = transform.root.name;

				// take damage and destroy prefabs
				enemy.TakeHit(damage, owner);
				Destroy(gameObject);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// check impact with layer "Obstacles"
		if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
		{
			Destroy(gameObject); // destroy fire ball when impact to Tilemap
			return;
		}
	}
}
