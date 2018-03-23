/*
* Author Walrys
* https://walrys.com
*
*/

using UnityEngine;


public class Bullet : MonoBehaviour {
	[SerializeField]
	string poolKey = "Bullet";
	public float damage = 1f;

	private void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag != "tower" && collider.gameObject.tag != "towerrange") {
			IAlive obj = collider.GetComponent<IAlive>();
			if (obj != null)
				collider.GetComponent<IAlive>().InflictDamage(damage);
			Destroy();
		}
	}
	public void Destroy() {
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
	}
}
