using UnityEngine;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour {
	// update score text with score
	void Update () {
		GetComponent<Text>().text = GameData.Score.ToString("");
	}
}
