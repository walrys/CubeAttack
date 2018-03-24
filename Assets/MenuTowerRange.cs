using UnityEngine;
using UnityEngine.UI;

public class MenuTowerRange : MonoBehaviour {
	void Update() {
		if(GetComponent<Text>() != null)
			GetComponent<Text>().text = GameData.TowerRangeSettings.ToString();
	}

	public void SetRange(float amount) {
		GameData.TowerRangeSettings = (int) amount;
	}
}
