using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour {
	public static WindowManager Instance;
	public GameObject WindowPrefab;
	
	private void Awake() {
		Instance = this;
	}
	
	public Window CreateWindow(string title, int minWidth, int maxWidth, int minHeight, int maxHeight, WindowType type) {
		Window newWindow = ((GameObject)Instantiate(WindowPrefab, Vector3.zero, Quaternion.identity, transform)).GetComponent<Window>();
		newWindow.TitleText.text = title;
		newWindow.MinimumWidth = minWidth;
		newWindow.MaximumWidth = maxWidth;
		newWindow.MinimumHeight = minHeight;
		newWindow.MaximumHeight = maxHeight;
		newWindow.Type = type;
		newWindow.GetComponent<RectTransform>().offsetMax = new Vector2(minWidth, minHeight);
		return newWindow;
	}
}
