using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, InteractElement
{
	[SerializeField] private float speed = 10f;
	[SerializeField] private float damage = 10f;
	[SerializeField] private float lifetime = 3f;
	private Rigidbody2D rb;
	private Vector2 direction;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
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
        
        transform.Translate(speed * Time.deltaTime * (Vector3)direction,Space.World);
    }


    private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check impact to hitbox with tag "EnemyHitbox"
		if (collision.gameObject.CompareTag("EnemyHitbox"))
		{
			EnemyBehaviour enemy = collision.GetComponentInParent<EnemyBehaviour>();
			if (enemy != null)
			{
				// take damage and destroy prefabs
				enemy.TakeHit(damage);
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
		if (collision.gameObject.CompareTag("Tornado"))
		{
			Destroy(gameObject); 
		}
	}
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void Interact(GameObject target)
    {
		
    }
}
