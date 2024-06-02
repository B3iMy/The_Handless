using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private enum State
    {
        roaming,
        chasing
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

		state = State.roaming;
	}

	private void Start()
	{
		StartCoroutine(RoamingRoutine());
	}

	private void Update()
	{
		if (state == State.roaming)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
			if (distanceToPlayer < chaseRadius)
			{
				state = State.chasing;
			}
		}
		else if (state == State.chasing)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
			if (distanceToPlayer > chaseRadius)
			{
				state = State.roaming;
			}
			else
			{
				enemyPathfinding.MoveTo(player.transform.position);
			}
		}
	}

	private IEnumerator RoamingRoutine()
    {
        while (state == State.roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
