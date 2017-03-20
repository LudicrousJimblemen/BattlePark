using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowTitleBar : MonoBehaviour {
	private Vector3 lastMousePosition;
	private RectTransform rectTransform;
	
	private void Awake() {
		rectTransform = transform.parent.GetComponent<RectTransform>();
	}
 
	private Vector3 GetClampedMousePosition() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.x = Mathf.Clamp(mousePosition.x, 0f, Screen.width);
		mousePosition.y = Mathf.Clamp(mousePosition.y, 0f, Screen.height);
 
		return mousePosition;
	}
 
	public void OnMouseDown() {
		lastMousePosition = GetClampedMousePosition();
	}
 
	public void OnMouseDrag() {
		transform.parent.position += GetClampedMousePosition() - lastMousePosition;
 
		Vector3 position = transform.parent.position;
		position.x = Mathf.Clamp(position.x, 0, Screen.width - rectTransform.rect.width);
		position.y = Mathf.Clamp(position.y, 0, Screen.height - rectTransform.rect.height);
 
		transform.parent.position = position;
 
		lastMousePosition = GetClampedMousePosition();
	}
}
