using System.Collections;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
	public enum State
	{
		Roaming,
		Chasing,
		Attacking,
		Stopped
	}

	private State state;
	private EnemyPathfinding enemyPathfinding;
	private GameObject player;

	[SerializeField] private float chaseRadius = 5f;
	[SerializeField] private float attackRadius = 1f; // New variable for attack radius
	[SerializeField] private float stopDuration = 2f;
	[SerializeField] private float wanderRadius = 5f;
	[SerializeField] private float wanderDistance = 2f;
	[SerializeField] private float wanderInterval = 2f;

	private void Awake()
	{
		InitializeComponents();
		state = State.Roaming;
	}

	private void Start()
	{
		StartCoroutine(RoamingRoutine());
	}

	private void Update()
	{
		if (player == null) return;

		float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
		UpdateStateBasedOnDistance(distanceToPlayer);
	}

	private void InitializeComponents()
	{
		enemyPathfinding = GetComponent<EnemyPathfinding>();
		if (enemyPathfinding == null)
		{
			Debug.LogError("EnemyPathfinding component not found on " + gameObject.name);
		}

		player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
		{
			Debug.LogError("Player not found in the scene.");
		}
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
					SetState(State.Stopped);
					StartCoroutine(StopRoutine());
				}
				else if (distanceToPlayer <= attackRadius) // Transition to Attacking if close enough
				{
					SetState(State.Attacking);
				}
				else
				{
					enemyPathfinding.Seek(player.transform);
				}
				break;
			case State.Attacking:
				if (distanceToPlayer > attackRadius)
				{
					SetState(State.Chasing);
				}
				break;
			case State.Stopped:
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
			enemyPathfinding.Stop();
		}
	}

	private IEnumerator RoamingRoutine()
	{
		while (state == State.Roaming)
		{
			enemyPathfinding.Wander(transform.position, wanderRadius, wanderDistance);
			yield return new WaitForSeconds(wanderInterval);
		}
	}

	private IEnumerator StopRoutine()
	{
		yield return new WaitForSeconds(stopDuration);
		if (state == State.Stopped)
		{
			SetState(State.Roaming);
		}
	}

	public State GetCurrentState()
	{
		return state;
	}
}
