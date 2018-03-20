using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour {
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

	#region Private Variables
	private ObjectPooler objectPooler;
	private float timerSlow = 0, timerFast = 0, enemySlowWidth, enemyFastWidth;
    private int gridSize = 1;
	private int enemyCountFast = 0, enemyCountSlow = 0;
    private Vector3 boardSize;
    private Vector3 enemySpawnPoint;
	private Vector3 enemySlowLastPos, enemyFastLastPos;
	#endregion
	
    void Start() {
		objectPooler = ObjectPooler.Instance;
        Bounds boardBounds = board.GetComponent<Renderer>().bounds;
        boardSize = boardBounds.size;
        enemySlowWidth = enemySlow.GetComponentInChildren<Renderer>().bounds.size.x;
        enemyFastWidth = enemyFast.GetComponentInChildren<Renderer>().bounds.size.x;
        enemySpawnPoint = board.transform.position + new Vector3(boardBounds.size.x / 2.0f, boardBounds.size.y / 2.0f, 0.5f);
    }
	
	void Update () {
        timerSlow += Time.deltaTime;
        timerFast += Time.deltaTime;
        // spawn enemy after previous enemy has moved + spawn delay
        if (enemyCountSlow < MAX_SLOW && timerSlow > spawnDelaySlow + 2 * enemySlow.GetComponent<Enemy>().GetRollSeconds()) {
			//spawnEnemySlow();
		}

        if (enemyCountFast < MAX_FAST && timerFast > spawnDelayFast + 2 * enemyFast.GetComponent<Enemy>().GetRollSeconds()) { 
            spawnEnemyFast();
        }

    }

    void spawnEnemySlow() {
        Vector3 startPosition = enemySpawnPoint + new Vector3(enemySlowWidth/2.0f, enemySlowWidth / 2.0f, (int)Random.Range(-boardSize.z/2f,boardSize.z/2f) * gridSize);
		while(enemySlowLastPos == startPosition) {
			startPosition = enemySpawnPoint + new Vector3(enemySlowWidth / 2.0f, enemySlowWidth / 2.0f, (int)Random.Range(-boardSize.z / 2f, boardSize.z / 2f) * gridSize);
		}
		enemySlowLastPos = startPosition;
		objectPooler.SpawnFromPool(enemySlowKey, startPosition, transform.rotation);
		enemyCountSlow++;
		timerSlow = 0;
	}

    void spawnEnemyFast() {
        Vector3 startPosition = enemySpawnPoint + new Vector3(enemyFastWidth/2.0f, enemyFastWidth / 2.0f, (int)Random.Range(-boardSize.z/2f, boardSize.z/2f) * gridSize);
		while (enemyFastLastPos == startPosition) {
			startPosition = enemySpawnPoint + new Vector3(enemyFastWidth / 2.0f, enemyFastWidth / 2.0f, (int)Random.Range(-boardSize.z / 2f, boardSize.z / 2f) * gridSize);
		}
		enemyFastLastPos = startPosition;
		objectPooler.SpawnFromPool(enemyFastKey, startPosition, transform.rotation);
		enemyCountFast++;
		timerFast = 0;
	}
}
