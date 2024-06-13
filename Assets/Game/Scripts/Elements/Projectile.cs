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



	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Kiểm tra va chạm với Hitbox có tag "EnemyHitbox"
		if (collision.gameObject.CompareTag("EnemyHitbox"))
		{
			EnemyBehaviour enemy = collision.GetComponentInParent<EnemyBehaviour>();
			if (enemy != null)
			{
				// Gây sát thương cho Enemy và hủy đạn
				enemy.TakeHit(damage);
				Destroy(gameObject);
			}
		}
	}
	
}
