using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject, IAlive {
	#region Variables
	[SerializeField][Range(1, 100)]
	protected int rollSpeed = 1;
	[SerializeField][Range(0f, 5f)]
	protected float rollDelay = 1f;
	[SerializeField][Range(0f, 5f)][LabelOverride("Health/Damage")]
	protected float health = 1;
	[SerializeField]
	protected int score = 10;

	protected string poolKey;

	protected GameObject pivotPoint;
	protected Transform pivot;

	protected float cubeSize;
	protected float timer = 0;
	protected float rollDuration;
	protected bool isMoving = false;
	protected int dodges = 0;
	#endregion
	public void OnObjectSpawn(string key) {
		poolKey = key;

		// reset state when recycled from object pool
		dodges = 0;
		isMoving = false;
		StopAllCoroutines();

		// get cube size
		cubeSize = GetComponent<Renderer>().bounds.size.x;

		// create child target object in the center
		pivotPoint = new GameObject("targetPoint");
		pivotPoint.transform.SetParent(transform);
		pivot = pivotPoint.transform;
		pivot.position = transform.position;
		pivot.rotation = Quaternion.identity;
	}

    void Update () {
        timer += Time.deltaTime;
		// roll every rollInterval seconds
		if (timer > rollDelay && !isMoving) {
			isMoving = true;
			Move();
        }
    }

	protected virtual void Move() {
		pivot.Translate(-cubeSize / 2, -cubeSize / 2, 0);
		StartCoroutine(DoRoll(Vector3.forward, 90.0f, rollSpeed));
	}

	protected IEnumerator DoRoll(Vector3 axis, float angle, float speed) {
		float aDuration = 1.0f / speed;
		float steps = Mathf.Ceil(aDuration * 100.0f);
		float stepAngle = angle / steps;

		// Rotate cube about pivot at every step by stepAngle
		for (var i = 1; i <= steps; i++) {
			transform.RotateAround(pivot.position, axis, stepAngle);
			yield return new WaitForSeconds(0.01f);
		}

		// move the targetpoint to the center of the cube
		pivot.position = transform.position;
		pivot.rotation = Quaternion.identity;

		// snap position and rotation
		Snap(transform);

		// reset move timer
		isMoving = false;
		timer = 0;
	}
	private void OnTriggerEnter(Collider collider) {
        switch (collider.gameObject.tag) {
            case "wall":
				//collider.gameObject.GetComponent<IAlive>().InflictDamage(health);
				Destroy(collider.gameObject);
				Destroy();
				break;
			case "tower":
				float received = collider.gameObject.GetComponent<Tower>().GetHealth();
				float dealt = health;
				InflictDamage(received);
				collider.gameObject.GetComponent<IAlive>().InflictDamage(dealt);
				break;
        }
	}
	
	public void InflictDamage(float dmg) {
		health -= dmg;
		if (health <= 0) {
			health = 0;
			Destroy();
		}
	}

	protected virtual void Destroy() {
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
		GameData.Score += score;
		//GameObject cubeBroken =  
		ObjectPooler.Instance.SpawnFromPool("SmashedFast", transform.position, transform.rotation);
		//float scale = 0.5f;
		//cubeBroken.transform.localScale -= new Vector3(scale, scale, scale);
		//foreach (Transform coob in cubeBroken.GetComponentsInChildren<Transform>()) {
		//	//scale = UnityEngine.Random.Range(0f, 0.1f);
		//	coob.localScale += new Vector3(scale, scale, scale);
	}


	void Snap(Transform obj) {
		int threshold = 4;

		// round off position to nearest 1/threshold
		Vector3 newPos;
		newPos.x = (float)Math.Round(obj.position.x * threshold) / threshold;
		newPos.y = (float)Math.Round(obj.position.y * threshold) / threshold;
		newPos.z = (float)Math.Round(obj.position.z * threshold) / threshold;
		obj.position = newPos;

		// Make sure the angles are snapping to 90 degrees.    
		Vector3 newRot = obj.eulerAngles;
		newRot.x = Mathf.Round(newRot.x / 90) * 90;
		newRot.y = Mathf.Round(newRot.y / 90) * 90;
		newRot.z = Mathf.Round(newRot.z / 90) * 90;
		obj.eulerAngles = newRot;
	}

	public float GetRollDelay() {
		return rollDelay;
	}

	public bool GetIsMoving() {
		return isMoving;
	}

	public float GetRollSeconds() {
		rollDuration = Mathf.Ceil(1.0f / rollSpeed * 100.0f) * 0.01f;
		return rollDelay + rollDuration;
	}
}
