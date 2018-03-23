﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour {
	//public float countdown = 2f;
	public float collectionCountdown = 0f;
	float timer = 0;
	float collectionTimer = 0;
	float height = 0;
	float maxHeight = 4f;
	float minSize = 0;
	float velocity = 0;
	Vector3 initialPos;

	// Use this for initialization
	public void Start () {
		initialPos = transform.localPosition;
		timer = 0;// Random.Range(0f, 2f);
		maxHeight = Random.Range(0f, 3f);
		collectionTimer = collectionCountdown + Random.Range(0f, 1f);
		minSize = Random.Range(0f, 2f);
		minSize = minSize <= 0.9f ? 0 : minSize;
		height = 0;
		velocity = 0;
		gameObject.GetComponent<Rigidbody>().useGravity = true;
	}

	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		collectionTimer -= Time.deltaTime;
		float reduction = 10f * Time.deltaTime;

		if (timer <= 0 && transform.localScale.x > 0 && transform.localScale.x >= minSize) {
			transform.localScale -= new Vector3(reduction, reduction, reduction);
			transform.localScale = transform.localScale.x <= 0 ? Vector3.zero : transform.localScale;
		}

		if(collectionTimer <= 0 && height < maxHeight) {
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			velocity += 0.5f * Time.deltaTime;
			height += velocity;
			transform.position += Vector3.up * velocity;
		}
		if(height >= maxHeight) {
			//Destroy(gameObject);
			transform.gameObject.SetActive(false);
		}
	}
}
