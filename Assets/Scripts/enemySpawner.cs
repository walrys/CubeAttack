using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {
    public GameObject enemySlow, enemyFast, board;
    float timer, enemyWidth;
    public delegate void spawnEnemy();
    public event spawnEnemy onEnemySpawn;
    // Use this for initialization
    void Start() {
        enemyWidth = enemySlow.GetComponentInChildren<Renderer>().bounds.size.x;
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        
        if(timer > 1)
        {
            spawnEnemySlow();
            timer = 0;
        }

	}

    void spawnEnemySlow() {
        Vector3 startPosition = board.transform.position + new Vector3(board.GetComponent<Renderer>().bounds.size.x / 2.0f + enemyWidth/2.0f, board.GetComponent<Renderer>().bounds.size.y, 0.5f + (int)Random.Range(-4,4));
        Instantiate(enemySlow, startPosition, transform.rotation);
    }

    void spawnEnemyFast() {

    }
}
