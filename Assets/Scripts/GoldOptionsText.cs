using UnityEngine;
using UnityEngine.UI;

public class GoldOptionsText : MonoBehaviour {
	void Update () {
		// update text displayed
		GetComponent<Text>().text = GameData.GoldSettings.ToString();
	}
}
