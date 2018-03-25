using UnityEngine;
using UnityEngine.UI;

public class UI_Gold : MonoBehaviour {
	public float flashDuration;
	float flashTimer = 0;
	bool isFlashing = false;
	Text goldText;
	Color initialColor, lerpedColor;

	#region Singleton
	public static UI_Gold Instance;
	private void Awake() {
		Instance = this;
	}
	#endregion

	private void Start() {
		goldText = GetComponent<Text>();
		initialColor = goldText.color;
	}

	void Update() {
		goldText.text = ((int) GameData.Gold).ToString();

		// flash text to white when not enough gold
		if(flashTimer >= 0) {
			flashTimer -= Time.deltaTime;
			lerpedColor = Color.Lerp(initialColor, Color.white, Mathf.PingPong(Time.time * 10 * flashDuration, 1));
			goldText.color = lerpedColor;
			isFlashing = true;
		}
		else {
			if (isFlashing) {
				isFlashing = false;
				goldText.color = initialColor;
			}
		}
	}

	public void Flash() {
		flashTimer = flashDuration;
	}
}
