﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScore : MonoBehaviour {
	private void Awake() {
		GameData.Score = 0;
		GameData.Gold = GameData.GoldSettings;
	}
}
