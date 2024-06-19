using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	public enum State
	{
		Roaming,
		Chasing,
		Stopped
	}

	private State state;
	private Rigidbody2D rb;
	private Transform playerTransform;
	private Vector2 roamingTarget;

	[SerializeField] private float chaseRadius = 5f;
	[SerializeField] private float stopRadius = 1f;
	[SerializeField] private float stopDuration = 2f;
	[SerializeField] private float wanderRadius = 5f;
	[SerializeField] private float wanderInterval = 2f;
	[SerializeField] private float moveSpeed = 3f;

	private Vector3 originalScale; // Added to store the original scale

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		originalScale = transform.localScale; // Store the original scale

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		if (playerTransform == null)
		{
			Debug.LogError("Player not found in the scene.");
		}

		state = State.Roaming;
		StartCoroutine(RoamingRoutine());
	}

	private void Update()
	{
		if (playerTransform == null) return;

		float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
		UpdateStateBasedOnDistance(distanceToPlayer);
	}

	private void UpdateStateBasedOnDistance(float distanceToPlayer)
	{
		switch (state)
		{
			case State.Roaming:
				if (distanceToPlayer < chaseRadius)
				{
					SetState(State.Chasing);
				}
				break;
			case State.Chasing:
				if (distanceToPlayer > chaseRadius)
				{
					SetState(State.Roaming);
				}
				else if (distanceToPlayer <= stopRadius)
				{
					SetState(State.Stopped);
					StartCoroutine(StopRoutine());
				}
				else
				{
					MoveTowards(playerTransform.position);
				}
				break;
			case State.Stopped:
				// Stopped state handled by coroutine
				break;
		}
	}

	private void SetState(State newState)
	{
		state = newState;
		if (newState == State.Roaming)
		{
			StartCoroutine(RoamingRoutine());
		}
		else if (newState == State.Stopped)
		{
			StopMovement();
		}
	}

	private void MoveTowards(Vector2 targetPosition)
	{
		Vector2 direction = (targetPosition - rb.position).normalized;
		rb.velocity = direction * moveSpeed;

		// Flip the GameObject based on movement direction
		if (direction.x < 0 && transform.localScale.x < 0)
		{
			flip();
		}
		else if (direction.x > 0 && transform.localScale.x > 0)
		{
			flip();
		}
	}

	private void flip()
	{
		Vector3 localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
	}

	private void StopMovement()
	{
		rb.velocity = Vector2.zero;
	}

	private IEnumerator RoamingRoutine()
	{
		while (state == State.Roaming)
		{
			// Randomly select a point within the wanderRadius radius
			roamingTarget = (Vector2)transform.position + Random.insideUnitCircle * wanderRadius;
			float wanderTime = Random.Range(1f, wanderInterval);

			MoveTowards(roamingTarget);

			yield return new WaitForSeconds(wanderTime);
		}
	}

	private IEnumerator StopRoutine()
	{
		yield return new WaitForSeconds(stopDuration);
		if (state == State.Stopped)
		{
			SetState(State.Chasing);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, chaseRadius);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, stopRadius);
	}
}
