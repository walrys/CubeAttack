using UnityEngine;

public class TowerRangeGuide : MonoBehaviour {
	// update the size of tower range visual guide
	void Start () {
		float radius = (int)GameData.TowerRangeSettings;
		transform.localScale = new Vector3(2 * radius + 1, transform.localScale.y, 2 * radius + 1);
	}
}
