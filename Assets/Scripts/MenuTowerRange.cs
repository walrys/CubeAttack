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

	private void Start() {
		// set slider initial value
		if (GetComponent<Slider>() != null)
			GetComponent<Slider>().value = GameData.TowerRangeSettings;
	}
}
