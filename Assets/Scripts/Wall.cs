using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IAlive {
	#region Serialized Variables
	[SerializeField]
	float initialHealth = 10;
	#endregion

	#region Variables
	float health;
	#endregion
	public void InflictDamage(float dmg) {
		health -= dmg;
		// do visual indication of lose health
		if (health <= 0)
			Destroy();
	}

	// Use this for initialization
	void Start () {
		health = initialHealth;
	}

	void Destroy() {
		// call game over
	}
}
