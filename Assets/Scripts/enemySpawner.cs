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

	[Space]

	[Header("Fast Enemy")]
	[SerializeField][LabelOverride("Object Pool Key")]
	private string enemyFastKey;
	[SerializeField][LabelOverride("Game Object")]
	private GameObject enemyFast;
	[SerializeField][LabelOverride("Spawn Delay")]
	private float spawnDelayFast = 0.5f;
	#endregion

	#region Private Variables
	private ObjectPooler objectPooler;
	private float timerSlow = 0, timerFast = 0, enemySlowWidth, enemyFastWidth;
    private int gridSize = 1;
    private Vector3 boardSize;
    private Vector3 enemySpawnPoint;
	private Vector3 enemySlowLastPos, enemyFastLastPos;
	#endregion

	//public delegate void spawnEnemy();
 //   public event spawnEnemy onEnemySpawn;
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
        if(timerSlow > spawnDelaySlow + 2 * enemySlow.GetComponent<Enemy>().GetRollSeconds()) {
			//spawnEnemySlow();
		}

        if (timerFast > spawnDelayFast + 2 * enemyFast.GetComponent<Enemy>().GetRollSeconds()) { 
            spawnEnemyFast();
        }

    }

    void spawnEnemySlow() {
        Vector3 startPosition = enemySpawnPoint + new Vector3(enemySlowWidth/2.0f, enemySlowWidth/ 2.0f, (int)Random.Range(-boardSize.z/2f,boardSize.z/2f) * gridSize);
		while(enemySlowLastPos == startPosition) {
			startPosition = enemySpawnPoint + new Vector3(enemySlowWidth / 2.0f, enemySlowWidth / 2.0f, (int)Random.Range(-boardSize.z / 2f, boardSize.z / 2f) * gridSize);
		}
		enemySlowLastPos = startPosition;
		objectPooler.SpawnFromPool(enemySlowKey, startPosition, transform.rotation);
		timerSlow = 0;
		//Instantiate(enemySlow, startPosition, transform.rotation);
	}

    void spawnEnemyFast() {
        Vector3 startPosition = enemySpawnPoint + new Vector3(enemyFastWidth/2.0f, enemyFastWidth / 2.0f, (int)Random.Range(-boardSize.z/2f, boardSize.z/2f) * gridSize);
		while (enemyFastLastPos == startPosition) {
			startPosition = enemySpawnPoint + new Vector3(enemyFastWidth / 2.0f, enemyFastWidth / 2.0f, (int)Random.Range(-boardSize.z / 2f, boardSize.z / 2f) * gridSize);
		}
		enemyFastLastPos = startPosition;
		objectPooler.SpawnFromPool(enemyFastKey, startPosition, transform.rotation);
		timerFast = 0;
		//Instantiate(enemyFast, startPosition, transform.rotation);
	}
}
