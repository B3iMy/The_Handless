using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private float spawnDelay = 2f;

	[SerializeField] private Vector2 spawnAreaMin;
	[SerializeField] private Vector2 spawnAreaMax;

	private void Start()
	{
		StartCoroutine(SpawnEnemyWithDelay());
	}

	private IEnumerator SpawnEnemyWithDelay()
	{
			yield return new WaitForSeconds(spawnDelay);
			SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		Vector2 spawnPosition = new Vector2(
			Random.Range(spawnAreaMin.x, spawnAreaMax.x),
			Random.Range(spawnAreaMin.y, spawnAreaMax.y)
		);

		GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
		spawnedEnemy.SetActive(true);
	}
}
