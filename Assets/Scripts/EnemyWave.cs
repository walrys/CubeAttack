using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyWave : MonoBehaviour {
	#region Serialized Fields
	[SerializeField]
	private GameObject board;

	[Space]

	[Header("Slow Enemy")]
	[SerializeField][LabelOverride("Object Pool Key")]
	private string enemySlowKey;
	[SerializeField][LabelOverride("Game Object")]
	private GameObject enemySlow;
	[SerializeField][LabelOverride("Spawn Delay")]
	private float spawnDelaySlow = 1f;
	[SerializeField][LabelOverride("Num of enemies")]
	private int MAX_SLOW = 1;

	[Space]

	[Header("Fast Enemy")]
	[SerializeField][LabelOverride("Object Pool Key")]
	private string enemyFastKey;
	[SerializeField][LabelOverride("Game Object")]
	private GameObject enemyFast;
	[SerializeField][LabelOverride("Spawn Delay")]
	private float spawnDelayFast = 0.5f;
	[SerializeField][LabelOverride("Num of enemies")]
	private int MAX_FAST = 1;
	#endregion

	[Space]

	public GameObject waveOverScreen;

	#region Private Variables
	private ObjectPooler objectPooler;
	private float timerSlow = 0, timerFast = 0, enemySlowWidth, enemyFastWidth;
    private int gridSize = 1;
	private int enemyCountFast = 0, enemyCountSlow = 0;
    private Vector3 boardSize;
    private Vector3 enemySpawnPoint;
	private Vector3 enemySlowLastPos, enemyFastLastPos;
	private float prevOffsetFast = -1, prevOffsetSlow = -1;
	private bool isWaveOver = false;
	#endregion
	
    void Start() {
		// Get Numbers from settings
		MAX_SLOW = GameData.NumEnemiesSlow;
		MAX_FAST = GameData.NumEnemiesFast;

		objectPooler = ObjectPooler.Instance;
        Bounds boardBounds = board.GetComponent<Renderer>().bounds;
        boardSize = boardBounds.size;
        enemySlowWidth = enemySlow.GetComponentInChildren<Renderer>().bounds.size.x;
        enemyFastWidth = enemyFast.GetComponentInChildren<Renderer>().bounds.size.x;
		// edge center of the board
        enemySpawnPoint = board.transform.position + new Vector3(boardBounds.size.x / 2f, boardBounds.size.y / 2f, 0);
	}
	
	void Update () {
        timerSlow += Time.deltaTime;
        timerFast += Time.deltaTime;
		// spawn enemy after previous enemy has moved + spawn delay
        if (enemyCountSlow < MAX_SLOW && timerSlow > spawnDelaySlow + enemySlow.GetComponent<Enemy>().GetRollSeconds()) {
			SpawnEnemySlow();
		}

        if (enemyCountFast < MAX_FAST && timerFast > spawnDelayFast + 2 * enemyFast.GetComponent<Enemy>().GetRollSeconds()) { 
            SpawnEnemyFast();
        }

		if (enemyCountSlow == MAX_SLOW && enemyCountFast == MAX_FAST
			&& objectPooler.IsPoolActive(enemyFastKey) && objectPooler.IsPoolActive(enemySlowKey)) {
			isWaveOver = true;
		}

		if (isWaveOver) {
			GameData.isGameOver = true;
			waveOverScreen.SetActive(true);
			Destroy(gameObject);
		}
    }

    void SpawnEnemySlow() {
		// starting from the bottom corner
		Vector3 bottomCornerPos = enemySpawnPoint + new Vector3(enemySlowWidth / 2f, enemySlowWidth / 2f, enemySlowWidth / 2f - boardSize.z / 2f);

		// total number of possible z positions
		int numOfPositions = ((int)boardSize.z) / gridSize * (int)(gridSize / enemySlowWidth);

		// pick random spawn position along edge
		float zOffset = Random.Range(0, numOfPositions);
		while (zOffset == prevOffsetSlow) {
			zOffset = Random.Range(0, numOfPositions);
		}
		prevOffsetSlow = zOffset;
		Vector3 spawnPosition = bottomCornerPos + Vector3.forward * zOffset * enemySlowWidth;

		objectPooler.SpawnFromPool(enemySlowKey, spawnPosition, transform.rotation);
		enemyCountSlow++;
		timerSlow = 0;
	}

    void SpawnEnemyFast() {
		// starting from the bottom corner
		Vector3 bottomCornerPos = enemySpawnPoint + new Vector3(enemyFastWidth/2f, enemyFastWidth / 2f, enemyFastWidth/2f - boardSize.z / 2f);
		
		// total number of possible z positions
		int numOfPositions = ((int) boardSize.z) / gridSize * (int) (gridSize / enemyFastWidth);

		// pick random spawn position along edge
		float zOffset = Random.Range(0, numOfPositions);
		while (zOffset == prevOffsetFast) {
			zOffset = Random.Range(0, numOfPositions);
		}
		prevOffsetFast = zOffset;
		Vector3 spawnPosition = bottomCornerPos + Vector3.forward * zOffset * enemyFastWidth;

		objectPooler.SpawnFromPool(enemyFastKey, spawnPosition, transform.rotation);
		enemyCountFast++;
		timerFast = 0;
	}
}
