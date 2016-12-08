using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace BattlePark.Menu {
	public enum WindowResizerType {
		Top,
		Bottom,
		Left,
		Right,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	[RequireComponent(typeof(EventTrigger))]
	public class WindowResizer : MonoBehaviour {
		public WindowResizerType Type;
		public RectTransform Target;
		
		public Vector2 MinimumDimensions;
		public Vector2 MaximumDimensions;
    
		private EventTrigger eventTrigger;
    
		private void Awake() {
			eventTrigger = GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			entry.callback.AddListener(data => {
				OnDrag((PointerEventData)data);
			});
			eventTrigger.triggers.Add(entry);
		}

		private void OnDrag(PointerEventData data) {
			RectTransform.Edge? horizontalEdge = null;
			RectTransform.Edge? verticalEdge = null;

			switch (Type) {
				case WindowResizerType.TopRight:
					horizontalEdge = RectTransform.Edge.Left;
					verticalEdge = RectTransform.Edge.Bottom;
					break;
				case WindowResizerType.Right:
					horizontalEdge = RectTransform.Edge.Left;
					break;
				case WindowResizerType.BottomRight:
					horizontalEdge = RectTransform.Edge.Left;
					verticalEdge = RectTransform.Edge.Top;
					break;
				case WindowResizerType.Bottom:
					verticalEdge = RectTransform.Edge.Top;
					break;
				case WindowResizerType.BottomLeft:
					horizontalEdge = RectTransform.Edge.Right;
					verticalEdge = RectTransform.Edge.Top;
					break;
				case WindowResizerType.Left:
					horizontalEdge = RectTransform.Edge.Right;
					break;
				case WindowResizerType.TopLeft:
					horizontalEdge = RectTransform.Edge.Right;
					verticalEdge = RectTransform.Edge.Bottom;
					break;
				case WindowResizerType.Top:
					verticalEdge = RectTransform.Edge.Bottom;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			if (horizontalEdge != null) {
				float deltaPivot = Target.pivot.x;
				
				if (horizontalEdge == RectTransform.Edge.Left) {
					Target.pivot = new Vector2(0f, Target.pivot.y);
					deltaPivot -= Target.pivot.x;
					
					Target.sizeDelta += new Vector2(data.delta.x, 0f);
				} else {
					Target.pivot = new Vector2(1f, Target.pivot.y);
					deltaPivot -= Target.pivot.x;
					
					Target.sizeDelta -= new Vector2(data.delta.x, 0f);
				}
				Target.position += new Vector3(Target.rect.width * deltaPivot * -0.5f, 0f, 0f);
			}
			if (verticalEdge != null) {
				float deltaPivot = Target.pivot.y;
				
				if (verticalEdge == RectTransform.Edge.Bottom) {
					Target.pivot = new Vector2(Target.pivot.x, 0f);
					deltaPivot -= Target.pivot.y;
					
					Target.sizeDelta += new Vector2(0f, data.delta.y * 2);
				} else {
					Target.pivot = new Vector2(Target.pivot.x, 1f);
					deltaPivot -= Target.pivot.y;
					
					Target.sizeDelta -= new Vector2(0f, data.delta.y * 2);
				}
				Target.position += new Vector3(0f, Target.rect.height * deltaPivot * -0.5f, 0f);
			}
			
			if (Target.sizeDelta.x < MinimumDimensions.x) {
				Target.sizeDelta = new Vector2(MinimumDimensions.x, Target.sizeDelta.y);
			}
			if (Target.sizeDelta.x > MaximumDimensions.x) {
				Target.sizeDelta = new Vector2(MaximumDimensions.x, Target.sizeDelta.y);
			}
			if (Target.sizeDelta.y < MinimumDimensions.y) {
				Target.sizeDelta = new Vector2(Target.sizeDelta.x, MinimumDimensions.y);
			}
			if (Target.sizeDelta.y > MaximumDimensions.y) {
				Target.sizeDelta = new Vector2(Target.sizeDelta.x, MaximumDimensions.y);
			}
		}
	}
}