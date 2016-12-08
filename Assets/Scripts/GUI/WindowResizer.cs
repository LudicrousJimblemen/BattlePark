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
		
		public Vector2 MinimumDimmensions;
		public Vector2 MaximumDimmensions;
    
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
				if (horizontalEdge == RectTransform.Edge.Right)
					Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)horizontalEdge, 
						Screen.width - Target.position.x - Target.pivot.x * Target.rect.width,
						Mathf.Clamp(Target.rect.width - data.delta.x, MinimumDimmensions.x, MaximumDimmensions.x));
				else
					Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)horizontalEdge, 
						Target.position.x - Target.pivot.x * Target.rect.width, 
						Mathf.Clamp(Target.rect.width + data.delta.x, MinimumDimmensions.x, MaximumDimmensions.x));
			}
			if (verticalEdge != null) {
				if (verticalEdge == RectTransform.Edge.Top)
					Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)verticalEdge, 
						Screen.height - Target.position.y - Target.pivot.y * Target.rect.height, 
						Mathf.Clamp(Target.rect.height - data.delta.y, MinimumDimmensions.y, MaximumDimmensions.y));
				else
					Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)verticalEdge, 
						Target.position.y - Target.pivot.y * Target.rect.height, 
						Mathf.Clamp(Target.rect.height + data.delta.y, MinimumDimmensions.y, MaximumDimmensions.y));
			}
		}
	}
}