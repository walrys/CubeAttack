using UnityEngine;

public class EnemySlow : Enemy {
	#region Variables
	[SerializeField]
	int viewRangeY = 10;
	[SerializeField][Range(0, 10)]
	int viewRangeX = 5;
	[SerializeField]
	float maxDodges = 3;
	float x, y, z;
	#endregion

	int CastPath(Vector3 start) {
		// prevent cube from rolling out of bounds
		float maxZ = Board.Instance.GetComponent<Renderer>().bounds.max.z;
		float minZ = Board.Instance.GetComponent<Renderer>().bounds.min.z;
		if (start.z > maxZ || start.z < minZ)
			return int.MaxValue;
				
		int pathWeight = 0;
		Vector3 direction = Vector3.left;
		Ray path = new Ray(start, direction);
		RaycastHit[] objectsInPath = Physics.RaycastAll(path, viewRangeY * cubeSize);
		//Debug.DrawLine(start, start + viewRangeY * cubeSize * direction, Color.red, 1f);

		foreach (RaycastHit hit in objectsInPath) {
			if(hit.collider.gameObject.tag == "towerrange" || hit.collider.gameObject.tag == "tower") {
				pathWeight++;
			}
		}

		return pathWeight;
	}

	int SafestPath() {
		// position of bottom center of cube
		Vector3 center = transform.position + new Vector3(cubeSize / 2f + 1, -cubeSize / 2f, 0);
		int safestPath = 0;
		int pathWeight = CastPath(center);
		int lowestWeight = pathWeight;

		// fire multiple parallel rays forward to check for safest path
		for (int i = viewRangeX; i >= -viewRangeX; i--) {
			Vector3 adjacent = center + new Vector3(cubeSize / 2f, -cubeSize / 2f, i * cubeSize);
			pathWeight = CastPath(adjacent);
			if (pathWeight < lowestWeight) {
				lowestWeight = pathWeight;
				safestPath = i;
			}
		}
		return safestPath;
	}

	protected override void Move() {
		if (dodges >= maxDodges) {
			// move forward if dodged more than maxDodges
			x = -cubeSize / 2;
			y = -cubeSize / 2;
			z = 0;
			dodges = 0;
		}
		else {
			int direction = SafestPath();
			// change path to one that is safer
			if (direction != 0) {
				direction = (int)Mathf.Sign(direction);
				x = 0;
				y = -cubeSize / 2;
				z = direction * cubeSize / 2;
				dodges++;
			}
			else {
				x = -cubeSize / 2;
				y = -cubeSize / 2;
				z = 0;
				dodges = 0;
			}
		}

		pivot.Translate(x, y, z);

		// calculate axis of rotation w.r.t pivot point
		Vector3 axis = Vector3.Normalize(new Vector3(z, 0, -x));
		StartCoroutine(DoRoll(axis, 90.0f, rollSpeed));
	}

	private void MoveRandom() {
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

	protected override void Destroy() {
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
		GameData.Score += score;
		ObjectPooler.Instance.SpawnFromPool("SmashedSlow", transform.position, transform.rotation);
	}
}
