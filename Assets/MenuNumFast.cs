using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNumFast : MonoBehaviour {
	void Update() {
		if (GetComponent<Text>() != null)
			GetComponent<Text>().text = GameData.NumEnemiesFast.ToString();
	}

	public void SetNumber(float amount) {
		GameData.NumEnemiesFast = (int) amount;
	}
}
