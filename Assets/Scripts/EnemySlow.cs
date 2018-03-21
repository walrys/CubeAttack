using UnityEngine;

public class EnemySlow : Enemy {
	#region Variables
	bool hasDodged = false;
	float x, y, z;
	#endregion

	protected override void Move() {
		// dodge left/right if path in front is tower / range
		if (hasDodged) {
			// move forward
			x = -cubeSize / 2;
			y = -cubeSize / 2;
			z = 0;
			hasDodged = false;
		}
		else {
			// dodge or move forward
			// if left or right lower weight
				// dodge
				hasDodged = true; // if dodged
			// else
				// move forward
		}
		
		// move in a random direction
		//bool xAxis = Random.value > 0.5f;
		//float x = xAxis ? (Random.Range(0, 2) * 2 - 1) * cubeSize / 2 : 0;
		//float y = -cubeSize / 2;
		//float z = xAxis ? 0 : (Random.Range(0, 2) * 2 - 1) * cubeSize / 2;
		
		pivot.Translate(x, y, z);

		// calculate axis of rotation w.r.t pivot point
		Vector3 axis = Vector3.Normalize(new Vector3(z, 0, -x));
		StartCoroutine(DoRoll(axis, 90.0f, rollSpeed));
	}
}
