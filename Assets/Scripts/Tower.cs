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
	int radius = 1; // radius of firing range
	[SerializeField]
	float fireDelay = 2f;
	[SerializeField][Range(0,10f)]
	float health = 3f;
	[SerializeField][Range(0,10f)]
	float damage = 3f;
	float bulletSpeed = 20f;
	[SerializeField]
	bool showRange = false;
	[SerializeField]
	Material rangeMaterial;
	#endregion

	#region Variables
	Vector2 pos2d;
	float countdown = 0;
	float fireRangeHeight = 0.1f; // height of firing range to be drawn
	Vector3 bulletStartPos;

	List<GameObject> enemiesInRange;
	#endregion

	private void Awake() {
		pos2d = new Vector2(0, 0);
	}

	public void OnObjectSpawn(string key) {
		// Get tower settings
		radius = (int) GameData.TowerRangeSettings;
		fireDelay = GameData.TowerDelaySettings;

		// Initialize variables
		enemiesInRange = new List<GameObject>();
		Vector3 pos = transform.position;
		bulletStartPos = new Vector3(pos.x, pos.y + GetComponent<Renderer>().bounds.size.y, pos.z);

		// create a cylinder and add as child
		GameObject fireRange = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		fireRange.transform.SetParent(transform);
		fireRange.transform.localPosition = Vector3.zero;
		fireRange.transform.rotation = Quaternion.identity;
		fireRange.transform.localScale = new Vector3(2 * radius + 0.95f, fireRangeHeight, 2 * radius + 0.95f);

		// set meshrenderer visibility
		fireRange.GetComponent<MeshRenderer>().enabled = showRange;
		if(rangeMaterial)
			fireRange.GetComponent<MeshRenderer>().material = rangeMaterial;
		fireRange.GetComponent<CapsuleCollider>().isTrigger = true;
		gameObject.AddComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().useGravity = false;
		fireRange.tag = "towerrange";
		fireRange.layer = 8; // Ignore mouse raycast;

		fireRange.name = "fireRange";

		
		countdown = fireDelay;
	}
	
	void Update () {
		countdown -= Time.deltaTime;

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
			bullet.GetComponent<Bullet>().damage = damage;
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
		Board.Instance.FreeGrid(pos2d);
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
	}

	public void SetGridPos(Vector2 pos) {
		pos2d = pos;
	}

	public float GetHealth() {
		return health;
	}
}
