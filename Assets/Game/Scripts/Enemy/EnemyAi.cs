using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private enum State
    {
        roaming
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;

	private void Awake()
	{
		enemyPathfinding = GetComponent<EnemyPathfinding>();
		if (enemyPathfinding == null)
		{
			Debug.LogError("EnemyPathfinding component not found on " + gameObject.name);
			return;
		}

		state = State.roaming;
	}

	private void Start()
	{
		StartCoroutine(RoamingRoutine());
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
