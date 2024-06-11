using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed = 10f;
	[SerializeField] private float damage = 10f;
	[SerializeField] private float lifetime = 3f;
	private Vector2 direction;

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

	

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{

			EnemyBehaviour enemy = collision.collider.GetComponent<EnemyBehaviour>();
			if (enemy != null)
			{
				// call Enemy take damage and destroy prefabs
				enemy.TakeHit(damage);

				Destroy(gameObject);
			}
		}
	}
}
