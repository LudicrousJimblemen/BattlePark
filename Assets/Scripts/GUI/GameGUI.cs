using System;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour {
	public static GameGUI Instance;
	
	public Text Money;
	
	private void Awake() {
		Instance = this;
	}
}
