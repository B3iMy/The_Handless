using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
	private enum State
	{
		Roaming,
		Chasing
	}

	private State state;
	private EnemyPathfinding enemyPathfinding;
	private GameObject player;
	[SerializeField] private float chaseRadius = 5f;

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
		float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

		if (state == State.Roaming && distanceToPlayer < chaseRadius)
		{
			state = State.Chasing;
			StopCoroutine(RoamingRoutine());
		}
		else if (state == State.Chasing && distanceToPlayer > chaseRadius)
		{
			state = State.Roaming;
			StartCoroutine(RoamingRoutine());
		}

		if (state == State.Chasing)
		{
			enemyPathfinding.MoveTo(player.transform.position);
		}
	}

	private IEnumerator RoamingRoutine()
	{
		while (state == State.Roaming)
		{
			Vector2 roamPosition = GetRoamingPosition();
			enemyPathfinding.MoveTo(roamPosition);
			yield return new WaitForSeconds(2f);
		}
	}

	private Vector2 GetRoamingPosition()
	{
		Vector2 roamDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
		return (Vector2)transform.position + roamDirection;
	}
}
