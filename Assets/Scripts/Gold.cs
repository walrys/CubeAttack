using UnityEngine;

public class Gold : MonoBehaviour {
	public float collectionDelay = 0f;
	float collectionTimer = 0;
	float timer = 0;
	float height = 0;
	float maxHeight = 4f;
	float minSize = 0;
	float velocity = 0;
	float goldAmount;

	public void Start () {
		// reset values at start
		timer = 0;
		height = 0;
		velocity = 0;

		// randomise height at which gold is collected
		maxHeight = Random.Range(0f, 3f);

		// randomise min size which gold will shrink down to
		minSize = Random.Range(0f, 2f);
		minSize = minSize <= 0.9f ? 0 : minSize;

		// set gold amount relative to size of cube
		goldAmount = transform.localScale.x;

		// randomise collection delay
		collectionTimer = collectionDelay + Random.Range(0f, 1f);
		gameObject.GetComponent<Rigidbody>().useGravity = true;
	}

	private void OnCollisionEnter(Collision collision) {
		// prevents wall from getting sitting on top of gold
		if(collision.collider.tag == "wall") {
			Collect();
		}
	}

	void Update () {
		timer -= Time.deltaTime;
		collectionTimer -= Time.deltaTime;
		float reduction = 10f * Time.deltaTime;

		// Scale down to random scale
		if (timer <= 0 && transform.localScale.x > 0 && transform.localScale.x >= minSize) {
			transform.localScale -= new Vector3(reduction, reduction, reduction);
			transform.localScale = transform.localScale.x <= 0 ? Vector3.zero : transform.localScale;
		}

		// Rise up to be collected
		if(collectionTimer <= 0 && height < maxHeight) {
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			velocity += 0.5f * Time.deltaTime;
			height += velocity;
			transform.position += Vector3.up * velocity;
		}

		// Collect gold if risen to max height
		if(height >= maxHeight) {
			Collect();
		}
	}

	void Collect() {
		// increase gold amount and "destroy" object
		transform.gameObject.SetActive(false);
		if (!GameData.isGameOver)
			GameData.Gold += goldAmount;
	}
}
