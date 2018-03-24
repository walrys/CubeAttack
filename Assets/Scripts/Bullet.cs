/*
* Author Walrys
* https://walrys.com
*
*/

using System.Collections;
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
		StartCoroutine(Fade());
	}

	IEnumerator Fade() {
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Vector3 scale = transform.localScale;
			transform.localScale = f * scale;
			yield return new WaitForSeconds(.005f);
		}

		Debug.Log("YAY");
		ObjectPooler.Instance.DestroyObject(poolKey, gameObject);
	}
}
