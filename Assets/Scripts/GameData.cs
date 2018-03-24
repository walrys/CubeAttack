/*
* Author Walrys
* https://walrys.com
*
*/

using UnityEngine;

public static class GameData {
	#region Variables
	public static bool isGameOver = false;
	public static int Score = 0;
	public static float Health = 50f;
	public static float Gold = 500f;
	#endregion

	#region Settings
	public static int GoldSettings = 500;
	public static int TowerRangeSettings = 1;
	public static int TowerDelaySettings = 2;
	public static int NumEnemiesSlow = 10;
	public static int NumEnemiesFast = 30;
	#endregion
}
