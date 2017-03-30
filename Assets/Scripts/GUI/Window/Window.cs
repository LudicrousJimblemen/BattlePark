using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum WindowType {
	Placeholder
}

public class Window : MonoBehaviour {
	public Image TitleBar;
	public Text TitleText;
	
	public int MinimumWidth = 120;
	public int MaximumWidth = 400;
	public int MinimumHeight = 120;
	public int MaximumHeight = 300;
	
	public WindowType Type;
	
	public void OnMouseDown() {
		transform.SetAsLastSibling(); // bring it to the front upon click
	}
}
