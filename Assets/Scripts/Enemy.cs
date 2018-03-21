using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject, IAlive {
	#region Variables
	[SerializeField][Range(1, 100)]
	protected int rollSpeed = 1;
	[SerializeField][Range(0f, 5f)]
	float rollDelay = 1f;
	[SerializeField][Range(0f, 5f)][LabelOverride("Health/Damage")]
	float health = 1;

	string poolKey;

	protected GameObject pivotPoint;
	protected Transform pivot;

	protected float cubeSize;
    float timer = 0;
	float rollDuration;
    bool isMoving = false;
	#endregion

	public void OnObjectSpawn(string key) {
		poolKey = key;

		// stop moving if recycled from object pool
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
            case "tower":
				collider.gameObject.GetComponent<IAlive>().InflictDamage(health);
                Destroy();
            return;
        }
	}

	private void CollideWall() {
		gameObject.SetActive(false);
		// TODO deal damage to wall
	}
	
	public void InflictDamage(float dmg) {
		health -= dmg;
		if (health <= 0)
			Destroy();
	}

	public void Destroy() {
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
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
