using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowResizer : MonoBehaviour {
	public enum ResizePoint {
		Top,
		Bottom,
		Left,
		Right,
		TopRight,
		TopLeft,
		BottomRight,
		BottomLeft
	}
	
	public ResizePoint Point;
	
	private Vector2 lastMousePosition;
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
		Vector2 clampedMouse = GetClampedMousePosition();
		
		switch (Point) {
			case ResizePoint.Top:
				rectTransform.offsetMax += new Vector2(0, clampedMouse.y) - new Vector2(0, lastMousePosition.y);
				break;
			case ResizePoint.Bottom:
				rectTransform.offsetMin += new Vector2(0, clampedMouse.y) - new Vector2(0, lastMousePosition.y);
				break;
			case ResizePoint.Left:
				rectTransform.offsetMin += new Vector2(clampedMouse.x, 0) - new Vector2(lastMousePosition.x, 0);
				break;
			case ResizePoint.Right:
				rectTransform.offsetMax += new Vector2(clampedMouse.x, 0) - new Vector2(lastMousePosition.x, 0);
				break;
			case ResizePoint.TopRight:
				rectTransform.offsetMax += clampedMouse - lastMousePosition;
				break;
			case ResizePoint.TopLeft:
				rectTransform.offsetMax += new Vector2(0, clampedMouse.y) - new Vector2(0, lastMousePosition.y);
				rectTransform.offsetMin += new Vector2(clampedMouse.x, 0) - new Vector2(lastMousePosition.x, 0);
				break;
			case ResizePoint.BottomRight:
				rectTransform.offsetMin += new Vector2(0, clampedMouse.y) - new Vector2(0, lastMousePosition.y);
				rectTransform.offsetMax += new Vector2(clampedMouse.x, 0) - new Vector2(lastMousePosition.x, 0);
				break;
			case ResizePoint.BottomLeft:
				rectTransform.offsetMin += clampedMouse - lastMousePosition;
				break;
		}
		
		lastMousePosition = GetClampedMousePosition();
	}
}
