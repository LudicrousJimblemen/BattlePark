using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattlePark;

public class WindowManager : MonoBehaviour {
	public GameObject WindowPrefab;
	public List<PropertyWindow> Windows;

	public PropertyWindow Focused = null;
	// Use this for initialization
	void Start () {
		Windows = new List<PropertyWindow>();
	}
	/*
	void Update() {
		if (Input.GetKeyDown (KeyCode.A)) {
			CreateWindow(null, "wokokodw");
		}
	}
	*/

	public void FocusWindow (PropertyWindow window) {
		print("clik");
		if (Focused != null) Focused.SendMessage("OnDefocus");
		Focused = window;
		Focused.SendMessage("OnFocus");
	}

	public void CloseWindow (PropertyWindow window) {
		Windows.Remove(window);
	}

	public PropertyWindow CreateWindow(GridObjectProperty[] Contents,string Name = "window") {
		GameObject windowObj = (GameObject)Instantiate(WindowPrefab,FindObjectOfType<Canvas>().transform.FindChild("Windows"));
		windowObj.GetComponent<RectTransform>().localPosition = Input.mousePosition - Vector3.Scale(Vector3.one,new Vector3(Screen.width,Screen.height))/2;
		PropertyWindow window = windowObj.GetComponent<PropertyWindow>();
		window.Initialize(200, Name, Contents);
		Windows.Add(window);
		return window;
	}

}
