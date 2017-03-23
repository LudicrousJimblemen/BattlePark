﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour {
	public static WindowManager Instance;
	
	public GameObject PlaceholderCameraPrefab;
	
	public GameObject WindowPrefab;
	public GameObject PlaceholderWindowPrefab;
	
	public List<Window> Windows;
	
	public Window SummonPlaceholderWindow(GridObject placeholderObject, Transform target) {
		if (Windows.Any(w => w.Type == WindowType.Placeholder)) {
			Windows.Remove(Windows.First(w => w.Type == WindowType.Placeholder));
		}
		
		FollowCamera followCamera = ((GameObject)Instantiate(PlaceholderCameraPrefab, Vector3.zero, Quaternion.identity)).GetComponent<FollowCamera>();
		followCamera.Target = target;
		followCamera.Output = new RenderTexture(190, 190, 16);
		
		Window newWindow = ((GameObject)Instantiate(PlaceholderWindowPrefab, Vector3.zero, Quaternion.identity, transform)).GetComponent<Window>();
		newWindow.TitleText.text = String.Format(LanguageManager.GetString("game.gui.placing"), LanguageManager.GetString(placeholderObject.ProperString));
		newWindow.GetComponentInChildren<RawImage>().texture = followCamera.Output;
		newWindow.Type = WindowType.Placeholder;
		
		Windows.Add(newWindow);
		
		return newWindow;
	}
	
	private void Awake() {
		Instance = this;
	}
}
