using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IAlive {
	#region Serialized Variables
	[SerializeField]
	float initialHealth = 10;
	#endregion

	public Color lerpedColor = Color.white;
	void Update() {
		lerpedColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time*10f, 1));
		//GetComponent<Renderer>().material.color = lerpedColor;
	}
	
	public void InflictDamage(float dmg) {
		GameData.Health -= dmg;
		// do visual indication of lose health
		if (GameData.Health <= 0) {
			// Game over
			Destroy();
		}
	}

	// Use this for initialization

	void Destroy() {
	}
}
