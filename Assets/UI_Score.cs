/*
* Author Walrys
* https://walrys.com
*
*/

using UnityEngine;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour {
	void Update () {
		GetComponent<Text>().text = GameData.Score.ToString("");
	}
}
