using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private List<GameObject> bossList;
	[SerializeField] private float spawnDelay = 2f;
	[SerializeField] private Vector2 spawnAreaMin;
	[SerializeField] private Vector2 spawnAreaMax;

	private int currentBossIndex = 0;
	private GameObject currentBoss;

	private void Start()
	{
		StartCoroutine(SpawnBossWithDelay(spawnDelay));
	}

	private IEnumerator SpawnBossWithDelay(float delay)
	{
			yield return new WaitForSeconds(delay);
			SpawnBoss();
	}

	private void SpawnBoss()
	{
		if (currentBossIndex < bossList.Count)
		{
			Vector2 spawnPosition = new Vector2(
				Random.Range(spawnAreaMin.x, spawnAreaMax.x),
				Random.Range(spawnAreaMin.y, spawnAreaMax.y)
			);

			currentBoss = Instantiate(bossList[currentBossIndex], spawnPosition, Quaternion.identity);
			currentBoss.SetActive(true);

			// Register the event when the boss is destroyed
			EnemyBehaviour enemyBehaviour = currentBoss.GetComponent<EnemyBehaviour>();
			if (enemyBehaviour != null)
			{
				enemyBehaviour.OnEnemyKilled += HandleBossKilled;
			}

			currentBossIndex++;
		}
	}

	private void HandleBossKilled()
	{
		// cancel the event
		EnemyBehaviour enemyBehaviour = currentBoss.GetComponent<EnemyBehaviour>();
		if (enemyBehaviour != null)
		{
			enemyBehaviour.OnEnemyKilled -= HandleBossKilled;
		}

		// Spawn another boss after 10 seconds
		StartCoroutine(SpawnBossWithDelay(10f));
	}
}
