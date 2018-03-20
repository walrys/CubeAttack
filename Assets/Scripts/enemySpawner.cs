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
	[SerializeField][LabelOverride("Game Object")]
	private GameObject enemySlow;
	[SerializeField][LabelOverride("Spawn Delay")]
	private float spawnDelaySlow = 1f;

	[Space]

	[Header("Fast Enemy")]
	[SerializeField][LabelOverride("Game Object")]
	private GameObject enemyFast;
	[SerializeField][LabelOverride("Spawn Delay")]
	private float spawnDelayFast = 0.5f;
	#endregion
	
	#region Private Variables
	private float timerSlow = 0, timerFast = 0, enemySlowWidth, enemyFastWidth;
    private int gridSize = 1;
    private Vector3 boardSize;
    private Vector3 enemySpawnPoint;
	#endregion

	public delegate void spawnEnemy();
    public event spawnEnemy onEnemySpawn;
    // Use this for initialization
    void Start() {
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
        if(timerSlow > spawnDelaySlow + enemySlow.GetComponent<Enemy>().GetRollDelay()
			&& enemySlow.GetComponent<Enemy>().GetIsMoving()) {
			//spawnEnemySlow();
			timerSlow = 0;
        }

        if (timerFast > spawnDelayFast + enemyFast.GetComponent<Enemy>().GetRollDelay()
			&& enemyFast.GetComponent<Enemy>().GetIsMoving())
        {
            spawnEnemyFast();
            timerFast = 0;
        }

    }

    void spawnEnemySlow() {
        Vector3 startPosition = enemySpawnPoint + new Vector3(enemySlowWidth/2.0f, enemySlowWidth/ 2.0f, (int)Random.Range(-boardSize.z/2f,boardSize.z/2f) * gridSize);
        Instantiate(enemySlow, startPosition, transform.rotation);
    }

    void spawnEnemyFast() {
        Vector3 startPosition = enemySpawnPoint + new Vector3(enemyFastWidth/2.0f, enemyFastWidth / 2.0f, (int)Random.Range(-boardSize.z/2f, boardSize.z/2f) * gridSize);
        Instantiate(enemyFast, startPosition, transform.rotation);
    }
}
