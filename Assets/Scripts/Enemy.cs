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
		// move pivot point to edge of cube to roll
		pivot.Translate(-cubeSize / 2, -cubeSize / 2, 0);
		StartCoroutine(DoRoll(Vector3.forward, rollSpeed));
	}

	protected IEnumerator DoRoll(Vector3 axis, float speed) {
		float aDuration = 1.0f / speed;
		float steps = Mathf.Ceil(aDuration * 100.0f);
		float stepAngle = 90f / steps;
		Vector3 direction = new Vector3(-axis.z, 0, axis.x);

		// wait if space in front is occupied by another cube
		RaycastHit hitInfo;
		Ray ray = new Ray(transform.position, direction);
		if (Physics.Raycast(ray, out hitInfo, cubeSize) && hitInfo.collider.tag == "enemy") {
			yield return new WaitForSeconds(0.5f);
		}

		else {
			// Rotate cube about pivot at every step by stepAngle
			for (var i = 1; i <= steps; i++) {
				transform.RotateAround(pivot.position, axis, stepAngle);
				yield return new WaitForSeconds(0.01f);
			}
		}

		// move the targetpoint back to the center of the cube
		pivot.position = transform.position;
		pivot.rotation = Quaternion.identity;

		// snap position and rotation
		Snap(transform);

		// reset move timer
		isMoving = false;
		timer = 0;
	}
	private void OnTriggerEnter(Collider collider) {
		// handle collision
		switch (collider.gameObject.tag) {
			case "wall":
				// destroy wall block and destroy self
				Destroy(collider.gameObject);
				Destroy();
				break;
			case "tower":
				// deal damage to tower and receive damage from tower
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
		// increase score when destroyed
		if(!GameData.isGameOver)
			GameData.Score += score;
		ObjectPooler.Instance.SpawnFromPool("SmashedFast", transform.position, transform.rotation);
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
