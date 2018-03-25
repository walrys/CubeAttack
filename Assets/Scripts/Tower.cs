/*
* Author Walrys
* https://walrys.com
*
*/

using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour, IAlive, IPooledObject  {
	#region Serialized variables
	[SerializeField]
	string poolKey = "Tower";
	[SerializeField]
	int fireRadius = 1;
	[SerializeField]
	float fireDelay = 2f;
	[SerializeField][Range(0,10f)]
	float health = 3f;
	[SerializeField][Range(0,10f)]
	float bulletDamage = 3f;
	float bulletSpeed = 20f;
	[SerializeField]
	bool showRange = false;
	[SerializeField]
	Material rangeMaterial;
	#endregion

	#region Variables
	Vector2 gridPosition;
	Vector3 bulletStartPos;
	float countdown = 0;
	float fireRangeHeight = 0.1f; // height of firing range to be drawn

	List<GameObject> enemiesInRange;
	#endregion

	private void Awake() {
		gridPosition = new Vector2(0, 0);
	}

	public void OnObjectSpawn(string key) {
		// Get tower settings
		fireRadius = (int) GameData.TowerRangeSettings;
		fireDelay = GameData.TowerDelaySettings;

		// Initialize variables
		enemiesInRange = new List<GameObject>();
		Vector3 pos = transform.position;
		bulletStartPos = new Vector3(pos.x, pos.y + GetComponent<Renderer>().bounds.size.y, pos.z);

		// Create a cylinder for range and add as child
		GameObject fireRange = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		fireRange.name = "fireRange";
		fireRange.tag = "towerrange";
		fireRange.layer = 8; // Ignore mouse raycast;
		fireRange.transform.localPosition = Vector3.zero;
		fireRange.transform.localScale = new Vector3(2 * fireRadius + 0.95f, fireRangeHeight, 2 * fireRadius + 0.95f);
		fireRange.transform.rotation = Quaternion.identity;
		fireRange.transform.SetParent(transform);

		// Set visibility of range
		fireRange.GetComponent<MeshRenderer>().enabled = showRange;
		if(rangeMaterial)
			fireRange.GetComponent<MeshRenderer>().material = rangeMaterial;

		// Set trigger collision only
		fireRange.GetComponent<CapsuleCollider>().isTrigger = true;

		// Remove physics
		gameObject.AddComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().useGravity = false;
		
		countdown = fireDelay;
	}
	
	void Update () {
		countdown -= Time.deltaTime;

		// fire every fire delay
		if(countdown <= 0) {
			Fire();
			countdown = fireDelay;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "enemy")
			enemiesInRange.Add(other.gameObject);
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "enemy")
			enemiesInRange.Remove(other.gameObject);
	}

	/**
	 * Returns nearest enemy from a list of enemies
	 */
	GameObject GetClosestEnemy(List<GameObject> enemies) {
		GameObject nearest = null;
		float nearestSqrDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (GameObject potentialTarget in enemies) {
			Vector3 vectorToTarget = potentialTarget.transform.position - currentPos;
			float sqrDist = vectorToTarget.sqrMagnitude;
			if (sqrDist < nearestSqrDist) {
				nearestSqrDist = sqrDist;
				nearest = potentialTarget;
			}
		}

		return nearest;
	}

	void Fire() {
		GameObject enemy = GetClosestEnemy(enemiesInRange);
		// remove all enemies that are inactive (dead) but in range
		while (enemy && !enemy.activeSelf) {
			enemiesInRange.Remove(enemy);
			enemy = GetClosestEnemy(enemiesInRange);
		}
		// shoot nearest enemy
;		if (!GameData.isGameOver && enemy) {
			GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet", bulletStartPos, Quaternion.identity);
			bullet.SetActive(true);
			bullet.GetComponent<Bullet>().damage = bulletDamage;
			Vector3 direction = Vector3.Normalize(enemy.transform.position - bullet.transform.position);
			bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
		}
	}

	public void InflictDamage(float dmg) {
		health -= dmg;
		if (health <= 0) {
			health = 0;
			Destroy();
		}
	}

	public void Destroy() {
		// free up grid position on board
		Board.Instance.FreeGrid(gridPosition);
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
	}

	public void SetGridPos(Vector2 pos) {
		gridPosition = pos;
	}

	public float GetHealth() {
		return health;
	}
}
