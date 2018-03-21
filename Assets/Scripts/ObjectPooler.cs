/*
* Author Walrys
* https://walrys.com
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPooler : MonoBehaviour {
	#region Serialized Fields
	[System.Serializable]
	public class Pool {
		public string key;
		public GameObject prefab;
		public int size;
	}
	// serialize a list of pools to be added into dictionary
	public List<Pool> pools;
	#endregion

	private Dictionary<string, Queue<GameObject>> poolDictionary;
	private Dictionary<string, int> activeItems;

	#region Singleton
	public static ObjectPooler Instance;
	private void Awake() {
		Instance = this;
	}
	#endregion

	void Start () {
		poolDictionary = new Dictionary<string, Queue<GameObject>>();
		activeItems = new Dictionary<string, int>();

		foreach (Pool pool in pools) {
			Queue<GameObject> objectPool = new Queue<GameObject>();
			for(int i=0; i<pool.size; i++) {
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}
			poolDictionary.Add(pool.key, objectPool);
			activeItems.Add(pool.key, 0);
		}
	}

	public GameObject SpawnFromPool(string key, Vector3 position, Quaternion rotation) {
		if (!poolDictionary.ContainsKey(key)) {
			Debug.LogWarning("Pool object '" + key + "' does not exist");
			return null;
		}
		GameObject spawn = poolDictionary[key].Dequeue();
		spawn.SetActive(true);
		spawn.transform.position = position;
		spawn.transform.rotation = rotation;
		
		// spawn object using interface
		IPooledObject pooledObject = spawn.GetComponent<IPooledObject>();
		if(pooledObject != null) {
			pooledObject.OnObjectSpawn(key);
		}

		// add object back too pool for reuse
		poolDictionary[key].Enqueue(spawn);
		activeItems[key]++;
		return spawn;
	}

	public bool DestroyObject(string key, GameObject obj) {
		if (!activeItems.ContainsKey(key)) {
			Debug.LogWarning("Pool object '" + key + "' does not exist");
			return false;
		}
		obj.SetActive(false);
		activeItems[key]--;
		return true;
	}

	public IEnumerator DestroyObject(string key, GameObject obj, float delay) {
		yield return new WaitForSeconds(delay);
		DestroyObject(key, obj);
	}

	public bool IsPoolActive(string key) {
		return activeItems[key] == 0;
	}

	public int ActivePool(string key) {
		return activeItems[key];
	}
}
