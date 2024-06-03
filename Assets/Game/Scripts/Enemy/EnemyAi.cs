using System.Collections;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
	private enum State
	{
		Roaming,
		Chasing,
		Stopped
	}

	private State state;
	private EnemyPathfinding enemyPathfinding;
	private GameObject player;
	[SerializeField] private float chaseRadius = 5f;
	[SerializeField] private float stopDuration = 2f;
	[SerializeField] private float wanderRadius = 5f;
	[SerializeField] private float wanderDistance = 2f;
	[SerializeField] private float wanderInterval = 2f;

	private void Awake()
	{
		enemyPathfinding = GetComponent<EnemyPathfinding>();
		if (enemyPathfinding == null)
		{
			Debug.LogError("EnemyPathfinding component not found on " + gameObject.name);
			return;
		}

		player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
		{
			Debug.LogError("Player not found in the scene.");
			return;
		}

		state = State.Roaming;
	}

	private void Start()
	{
		StartCoroutine(RoamingRoutine());
	}

	private void Update()
	{
		if (player == null)
		{
			Debug.LogError("Player is still null in Update");
			return;
		}

		float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

		switch (state)
		{
			case State.Roaming:
				// Khi trong trạng thái Roaming, triển khai hành vi Wander
				if (distanceToPlayer < chaseRadius)
				{
					state = State.Chasing;
					StopCoroutine(RoamingRoutine());
				}
				break;

			case State.Chasing:
				if (distanceToPlayer > chaseRadius)
				{
					state = State.Stopped;
					enemyPathfinding.Stop();
					StartCoroutine(StopRoutine());
				}
				else
				{
					// Khi trong trạng thái Chasing, triển khai hành vi Seek để đuổi theo Player
					enemyPathfinding.Seek(player.transform);
				}
				break;

			case State.Stopped:
				break;
		}
	}

	private IEnumerator RoamingRoutine()
	{
		while (state == State.Roaming)
		{
			// Trạng thái Roaming, tiếp tục Wander
			enemyPathfinding.Wander(transform.position, wanderRadius, wanderDistance);
			yield return new WaitForSeconds(wanderInterval);
		}
	}

	private IEnumerator StopRoutine()
	{
		yield return new WaitForSeconds(stopDuration);
		if (state == State.Stopped)
		{
			state = State.Roaming;
			StartCoroutine(RoamingRoutine());
		}
	}
}
