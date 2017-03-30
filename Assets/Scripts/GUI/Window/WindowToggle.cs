using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowToggle : MonoBehaviour {
	public Action ToggleAction;
	public void OnMouseDown() {
		print ("togg");
		if (ToggleAction != null) {
			ToggleAction();
		}
	}
}
