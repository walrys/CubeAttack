using UnityEngine;

public class EnemySlow : Enemy {
	protected override void Move() {
		// move in a random direction
		bool xAxis = Random.value > 0.5f;
		float x = xAxis ? (Random.Range(0, 2) * 2 - 1) * cubeSize / 2 : 0;
		float y = -cubeSize / 2;
		float z = xAxis ? 0 : (Random.Range(0, 2) * 2 - 1) * cubeSize / 2;
		
		pivot.Translate(x, y, z);

		// calculate axis of rotation w.r.t pivot point
		Vector3 axis = Vector3.Normalize(new Vector3(z, 0, -x));
		StartCoroutine(DoRoll(axis, 90.0f, rollSpeed));
	}
}
