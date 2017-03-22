using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Window : MonoBehaviour {
	public Image TitleBar;
	public Text TitleText;
	
	public int MinimumWidth = 120;
	public int MaximumWidth = 400;
	public int MinimumHeight = 120;
	public int MaximumHeight = 300;
}
