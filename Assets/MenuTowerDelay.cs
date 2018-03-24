using UnityEngine;
using UnityEngine.UI;

public class MenuTowerDelay : MonoBehaviour {
	void Update() {
		if (GetComponent<Text>() != null)
			GetComponent<Text>().text = GameData.TowerDelaySettings.ToString();
	}

	public void SetDelay(float amount) {
		GameData.TowerDelaySettings = (int) amount;
	}
}
