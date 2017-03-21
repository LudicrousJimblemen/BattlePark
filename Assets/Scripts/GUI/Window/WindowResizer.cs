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
	
	private Window window;
	private RectTransform parent;
	
	private void Awake() {
		window = transform.parent.GetComponent<Window>();
		parent = transform.parent.GetComponent<RectTransform>();
	}
 
	private Vector3 GetClampedMousePosition() {
		Vector3 mousePosition = Input.mousePosition;
		
		if (Point == ResizePoint.Top || Point == ResizePoint.TopLeft || Point == ResizePoint.TopRight) {
			mousePosition.y = Mathf.Clamp(mousePosition.y, parent.position.y + window.MinimumHeight, parent.position.y + window.MaximumHeight);
		}
		if (Point == ResizePoint.Bottom || Point == ResizePoint.BottomLeft || Point == ResizePoint.BottomRight) {
			mousePosition.y = Mathf.Clamp(mousePosition.y, parent.position.y + parent.rect.height - window.MaximumHeight, parent.position.y + parent.rect.height - window.MinimumHeight);
		}
		if (Point == ResizePoint.Left || Point == ResizePoint.BottomLeft || Point == ResizePoint.TopLeft) {
			mousePosition.x = Mathf.Clamp(mousePosition.x, parent.position.x + parent.rect.width - window.MaximumWidth, parent.position.x + parent.rect.width - window.MinimumWidth);
		}
		if (Point == ResizePoint.Right || Point == ResizePoint.BottomRight || Point == ResizePoint.TopRight) {
			mousePosition.x = Mathf.Clamp(mousePosition.x, parent.position.x + window.MinimumWidth, parent.position.x + window.MaximumWidth);
		}
		
		mousePosition.x = Mathf.Clamp(mousePosition.x, 0f, Screen.width);
		mousePosition.y = Mathf.Clamp(mousePosition.y, 0f, Screen.height);
		
		return mousePosition;
	}
 
	public void OnMouseDown() {
		lastMousePosition = GetClampedMousePosition();
	}
 
	public void OnMouseDrag() {
		Vector2 mouse = Input.mousePosition;
		
		if (Point == ResizePoint.Top || Point == ResizePoint.TopLeft || Point == ResizePoint.TopRight) {
			parent.offsetMax +=
				new Vector2(
					0,
					Mathf.Clamp(mouse.y - lastMousePosition.y, -parent.rect.height + window.MinimumHeight, Screen.width - parent.rect.width - window.MaximumWidth + parent.position.y));
		}
		if (Point == ResizePoint.Bottom || Point == ResizePoint.BottomLeft || Point == ResizePoint.BottomRight) {
			parent.offsetMin +=
				new Vector2(
					0,
					Mathf.Clamp(mouse.y - lastMousePosition.y, -parent.position.y, parent.position.y + parent.rect.height));
		}
		if (Point == ResizePoint.Left || Point == ResizePoint.BottomLeft || Point == ResizePoint.TopLeft) {
			parent.offsetMin +=
				new Vector2(
					Mathf.Clamp(mouse.x - lastMousePosition.x, -parent.position.x, parent.position.x + parent.rect.width),
					0);
		}
		if (Point == ResizePoint.Right || Point == ResizePoint.BottomRight || Point == ResizePoint.TopRight) {
			parent.offsetMax +=
				new Vector2(
					Mathf.Clamp(mouse.x - lastMousePosition.x, -parent.rect.width + window.MinimumWidth, Screen.width - parent.rect.width - window.MaximumWidth + parent.position.x),
					0);
		}
 
		Vector3 position = parent.position;
		position.x = Mathf.Clamp(position.x, 0, Screen.width - parent.rect.width);
		position.y = Mathf.Clamp(position.y, 0, Screen.height - parent.rect.height);
		parent.position = position;
		
		lastMousePosition = GetClampedMousePosition();
	}
}
