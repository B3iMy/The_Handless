using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 2f;
	private Rigidbody2D rb;
	private Vector2 moveDir;
	private Vector3 originalScale;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		originalScale = transform.localScale;
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

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
}
