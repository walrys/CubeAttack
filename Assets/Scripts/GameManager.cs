using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	#region Singleton
	public static GameManager Instance;
	private void Awake() {
		Instance = this;
		DontDestroyOnLoad(this.gameObject);
	}
	#endregion

	public GameObject menu;
	float slowFactor = 0.01f;
	[System.NonSerialized]
	public bool isGameOver = false;

	private void Start() {
		isGameOver = false;
	}

	public void GameOver() {
		isGameOver = true;
		Invoke("ShowGameOverMenu", 3f);
	}

	void ShowGameOverMenu() {
		menu.SetActive(true);
	}
}
