using UnityEngine;
using UnityEngine.UI;

public class GoldSlider : MonoBehaviour {
	public void SetGold(float amount) {
		GameData.GoldSettings = (int) amount;
		GameData.Gold = amount;
	}
	private void Start() {
		GetComponent<Slider>().value = GameData.GoldSettings;
		GameData.Gold = GameData.GoldSettings;
	}
}
