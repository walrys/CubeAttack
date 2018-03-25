using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseBorder : MonoBehaviour {
	public Camera camera;
	Transform cubePos;
	float zoomSpeed = 3;
	bool enemyPastBorder = false;
	float newX;
	Vector3 newRotation;

	private void OnTriggerEnter(Collider other) {
		// trigger GameOver() once
		if(other.tag == "enemy" && !enemyPastBorder) {
			cubePos = other.transform;
			enemyPastBorder = true;
			GameManager.Instance.GameOver();
		}
	}

	private void FixedUpdate() {
		if (!enemyPastBorder)
			return;
		// dolly camera to follow enemy position x
		newX = Mathf.Lerp(camera.transform.position.x, cubePos.position.x, Time.deltaTime);
		newRotation = Vector3.Lerp(camera.transform.eulerAngles, new Vector3(48.5f, 0, 0), 0.5f * Time.deltaTime);
		camera.transform.position = new Vector3(newX, camera.transform.position.y, camera.transform.position.z);
		camera.transform.eulerAngles = newRotation;
		//camera.transform.LookAt(cubePos);

		// zoom camera into board
		if (camera.fieldOfView > 28) {
			camera.fieldOfView -= zoomSpeed*Time.deltaTime;
		}
	}
}
