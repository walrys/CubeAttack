﻿using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
	#region Variables
	[SerializeField][Range(1, 10)]
	protected int rollSpeed = 1;
	[SerializeField][Range(0f, 5f)]
	private float rollDelay = 1f;

	protected GameObject pivotPoint;
	protected Transform pivot;

	protected float cubeSize;
    private float timer = 0;
	private float rollDuration;
    private bool isMoving = false;
	#endregion

	void Start () {
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
		Debug.Log("OnCollisionEnter");
		Debug.Log(collider.gameObject.name);
		if (collider.gameObject.name == "Wall")
			CollideWall();
	}
	private void OnTriggerStay(Collider collider) {
		Debug.Log("OnCollisionStay");
		Debug.Log(collider.gameObject.name);
		if (collider.gameObject.name == "Wall")
			CollideWall();
	}

	private void CollideWall() {
		Destroy(gameObject);
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
}
