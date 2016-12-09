using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class WindowFocusObject : MonoBehaviour {
	void Start () {
		WindowManager manager = FindObjectOfType<WindowManager>();
	}

	public void InterceptOnPointerDown () {
		FindObjectOfType<WindowManager>().FocusWindow(transform.parent.GetComponent<PropertyWindow>());
	}
}
