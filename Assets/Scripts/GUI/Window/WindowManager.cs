using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {
	public static WindowManager Instance;

	public List<Window> Windows = new List<Window>();
	
	private void Awake() {
		Instance = this;
	}
}
