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
		if (collision.CompareTag("Player"))
		{
			PlayerBehaviour player = collision.GetComponentInParent<PlayerBehaviour>();
			if (player != null)
			{
				// Apply damage to the player
				player.TakeHit(damage);
				
				Debug.Log("Nhan damage tu Laser");
			}

			// Destroy the laser after hitting an object
			Destroy(gameObject);
		}
	}
}