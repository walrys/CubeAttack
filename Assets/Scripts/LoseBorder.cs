using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseBorder : MonoBehaviour {
	public Camera camera;
	Transform cubePos;
	float zoomSpeed = 3;
	bool isLost = false;
	float newX;
	Vector3 newRotation;
	private void OnTriggerEnter(Collider other) {
		if(other.tag == "enemy") {
			if (!isLost) {
				cubePos = other.transform;
				isLost = true;
				GameManager.Instance.GameOver();
			}
			else {
				//other.gameObject.SetActive(false);
			}
		}
	}

	private void FixedUpdate() {
		if (!isLost)
			return;
		newX = Mathf.Lerp(camera.transform.position.x, cubePos.position.x, Time.deltaTime);
		newRotation = Vector3.Lerp(camera.transform.eulerAngles, new Vector3(48.5f, 0, 0), 0.5f * Time.deltaTime);

		camera.transform.position = new Vector3(newX, camera.transform.position.y, camera.transform.position.z);
		camera.transform.eulerAngles = newRotation;
		//camera.transform.LookAt(cubePos);

		if (camera.fieldOfView > 28) {
			camera.fieldOfView -= zoomSpeed*Time.deltaTime;
		}
	}
}
