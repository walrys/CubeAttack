/*
* Author Walrys
* https://walrys.com
*
*/

using UnityEngine;
using UnityEngine.UI;


public class GoldOptionsText : MonoBehaviour {
	void Update () {
		GetComponent<Text>().text = ((int) GameData.Gold).ToString();
	}
}
