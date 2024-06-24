using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
	[SerializeField] private float damage = 10f;
	[SerializeField] private float lifetime = 2f;

	private void Start()
	{
		// Destroy the laser after a certain time
		Destroy(gameObject, lifetime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check impact to hitbox with tag "EnemyHitbox"
		if (collision.gameObject.CompareTag("Player"))
		{
			PlayerBehaviour player = collision.GetComponentInParent<PlayerBehaviour>();
			if (player != null)
			{
				// take damage and destroy prefabs
				player.TakeHit(damage);
				Debug.Log("Player nhan damage laser");
				Destroy(gameObject);
			}	
		}
		else if (collision.gameObject.CompareTag("Obstacles"))
		{
			Destroy(gameObject);
			Debug.Log("Tram vao Tilemap va bien mat");
			return;
		}
	}
}
