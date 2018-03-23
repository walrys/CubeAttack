using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShattered : MonoBehaviour, IPooledObject {
	List<Vector3> initialPosition;
	List<Quaternion> initialRotation;
	List<Vector3> initialScale;
	public string poolKey;
	float timer = 0;
	void Awake() {
		if (initialPosition == null) {
			initialPosition = new List<Vector3>();
			initialRotation = new List<Quaternion>();
			initialScale = new List<Vector3>();
			for (int i = 0; i < transform.childCount; i++) {
				initialPosition.Add(transform.GetChild(i).transform.localPosition);
				initialRotation.Add(transform.GetChild(i).transform.localRotation);
				initialScale.Add(transform.GetChild(i).transform.localScale);
			}
			Debug.Log(initialPosition);
		}
	}
	public void OnObjectSpawn(string key) {
		Debug.Log("reset");
		timer = 0f;
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(true);
			transform.GetChild(i).gameObject.GetComponent<Shrink>().Start();
			transform.GetChild(i).transform.localPosition = initialPosition[i];
			transform.GetChild(i).transform.localRotation = initialRotation[i];
			transform.GetChild(i).transform.localScale = Vector3.one;
		}
	}

	public void Destroy() {
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
	}
}
