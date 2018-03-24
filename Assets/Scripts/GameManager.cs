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

	private void Start() {
		GameData.isGameOver = false;
	}

	public void GameOver() {
		GameData.isGameOver = true;
		Invoke("ShowGameOverMenu", 3f);
	}

	void ShowGameOverMenu() {
		if(menu != null)
			menu.SetActive(true);
	}
}
