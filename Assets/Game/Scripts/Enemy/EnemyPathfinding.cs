using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyPathfinding : MonoBehaviour
{
	[SerializeField] protected Golem entity;

	private float moveSpeed;
	private Rigidbody2D rb;
	private Vector2 moveDir;
	private Vector3 originalScale;

	// Target của Seek
	private Transform seekTarget;
	private LayerMask obstacleLayer;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		originalScale = transform.localScale;

		if (rb == null)
		{
			Debug.LogError("Rigidbody2D is null on " + gameObject.name);
		}

		// Xác định layer của các chướng ngại vật
		obstacleLayer = LayerMask.GetMask("Obstacles");

		if (entity != null)
		{
			moveSpeed = entity.speed_movement;
		}
		else
		{
			Debug.LogError("ScriptableEntity is not assigned on " + gameObject.name);
		}
	}

	private void FixedUpdate()
	{
		// Di chuyển theo hướng moveDir
		Vector2 newPosition = rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime);

		// Kiểm tra va chạm với chướng ngại vật
		RaycastHit2D hit = Physics2D.Raycast(rb.position, moveDir, moveDir.magnitude * moveSpeed * Time.fixedDeltaTime, obstacleLayer);
		if (hit.collider != null)
		{
			// Chuyển hướng ngẫu nhiên nếu gặp chướng ngại vật
			moveDir = Random.insideUnitCircle.normalized;
		}

		rb.MovePosition(newPosition);

		// Vẽ đường thẳng để kiểm tra hướng di chuyển
		Debug.DrawRay(transform.position, moveDir * moveSpeed * Time.fixedDeltaTime, Color.red, 0.5f);

		// Flip hình ảnh nếu cần
		if (moveDir.x < 0 && transform.localScale.x < 0)
		{
			flip();
		}
		else if (moveDir.x > 0 && transform.localScale.x > 0)
		{
			flip();
		}
	}

	public void MoveTo(Vector2 targetPosition)
	{
		moveDir = (targetPosition - rb.position).normalized;
	}

	// Triển khai hành vi Seek
	public void Seek(Transform target)
	{
		seekTarget = target;
		if (seekTarget != null)
		{
			Vector2 desiredVelocity = (seekTarget.position - transform.position).normalized * moveSpeed;
			moveDir = desiredVelocity.normalized;
		}
		else
		{
			Debug.LogError("Seek target is null");
		}
	}

	// Triển khai hành vi Wander
	public void Wander(Vector2 wanderCenter, float wanderRadius, float wanderDistance)
	{
		Vector2 wanderTarget = wanderCenter + Random.insideUnitCircle.normalized * wanderRadius;
		Vector2 directionToWanderTarget = (wanderTarget - (Vector2)transform.position).normalized;
		Vector2 targetPosition = (Vector2)transform.position + directionToWanderTarget * wanderDistance;

		// Vẽ đường thẳng để kiểm tra hướng di chuyển
		Debug.DrawRay(transform.position, directionToWanderTarget * wanderDistance, Color.red, 1f);

		MoveTo(targetPosition);
	}

	private void flip()
	{
		Vector3 localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
	}

	public void Stop()
	{
		moveDir = Vector2.zero;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Tránh va chạm bằng cách đảo ngược hướng di chuyển
		if (collision.gameObject.GetComponent<TilemapCollider2D>() != null)
		{
			moveDir = -moveDir;
		}
	}
}
