using UnityEngine;
using UnityEngine.UI;


public class GoldOptionsText : MonoBehaviour {
	void Update () {
		GetComponent<Text>().text = GameData.GoldSettings.ToString();
	}
}
