using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScore : MonoBehaviour {
	// Reset score and gold UI values on next run
	private void Awake() {
		GameData.Score = 0;
		GameData.Gold = GameData.GoldSettings;
	}
}
