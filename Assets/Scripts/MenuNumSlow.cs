using UnityEngine;
using UnityEngine.UI;

public class MenuNumSlow : MonoBehaviour {
	void Update() {
		if (GetComponent<Text>() != null)
			GetComponent<Text>().text = GameData.NumEnemiesSlow.ToString();
	}

	public void SetNumber(float amount) {
		GameData.NumEnemiesSlow = (int)amount;
	}

	private void Start() {
		// set slider initial value
		if (GetComponent<Slider>() != null)
			GetComponent<Slider>().value = GameData.NumEnemiesSlow;
	}
}
