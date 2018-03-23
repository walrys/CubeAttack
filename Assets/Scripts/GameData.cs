/*
* Author Walrys
* https://walrys.com
*
*/

using UnityEngine;

public static class GameData {
	#region Variables
	public static int Score = 0;
	public static float Health = 50f;
	public static float Gold = 500f;
	#endregion

	public static void UpdateGold() {
		Gold += 1;
	}
}
