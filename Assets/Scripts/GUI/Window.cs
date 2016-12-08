using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattlePark.Menu {
	[RequireComponent(typeof(EventTrigger))]
	public class Window : MonoBehaviour {
		public GameObject ResizerPrefab;
		
		public Vector2 MinimumDimensions;
		public Vector2 MaximumDimensions;
		
		public RectTransform Target;
		private EventTrigger eventTrigger;
		
		private void Awake() {
			eventTrigger = GetComponent<EventTrigger>();
			
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			entry.callback.AddListener(data => {
				OnDrag((PointerEventData)data);
			});
			
			eventTrigger.triggers.Add(entry);
			
			for (int i = 0; i < 8; i++) {
				GameObject resizer = (GameObject)Instantiate(ResizerPrefab, Vector3.zero, Quaternion.identity, transform.parent);
				
				WindowResizer windowResizer = resizer.GetComponent<WindowResizer>();
				windowResizer.Type = (WindowResizerType)i;
				windowResizer.Target = Target;
				windowResizer.MinimumDimensions = MinimumDimensions;
				windowResizer.MaximumDimensions = MaximumDimensions;
				
				RectTransform rectTransform = resizer.GetComponent<RectTransform>();
				
				switch ((WindowResizerType)i) {
					case WindowResizerType.Top:
						rectTransform.anchorMin = new Vector2(0f, 1f);
						rectTransform.anchorMax = new Vector2(1f, 1f);
						rectTransform.sizeDelta = new Vector2(-16f, 16f);
						break;
						
					case WindowResizerType.Bottom:
						rectTransform.anchorMin = new Vector2(0f, 0f);
						rectTransform.anchorMax = new Vector2(1f, 0f);
						rectTransform.sizeDelta = new Vector2(-16f, 16f);
						break;
						
					case WindowResizerType.Left:
						rectTransform.anchorMin = new Vector2(0f, 0f);
						rectTransform.anchorMax = new Vector2(0f, 1f);
						rectTransform.sizeDelta = new Vector2(16f, -16f);
						break;
						
					case WindowResizerType.Right:
						rectTransform.anchorMin = new Vector2(1f, 0f);
						rectTransform.anchorMax = new Vector2(1f, 1f);
						rectTransform.sizeDelta = new Vector2(16f, -16f);
						break;
						
					case WindowResizerType.TopLeft:
						rectTransform.anchorMin = new Vector2(0f, 1f);
						rectTransform.anchorMax = new Vector2(0f, 1f);
						rectTransform.sizeDelta = new Vector2(16f, 16f);
						break;
						
					case WindowResizerType.TopRight:
						rectTransform.anchorMin = new Vector2(1f, 1f);
						rectTransform.anchorMax = new Vector2(1f, 1f);
						rectTransform.sizeDelta = new Vector2(16f, 16f);
						break;
						
					case WindowResizerType.BottomLeft:
						rectTransform.anchorMin = new Vector2(0f, 0f);
						rectTransform.anchorMax = new Vector2(0f, 0f);
						rectTransform.sizeDelta = new Vector2(16f, 16f);
						break;
						
					case WindowResizerType.BottomRight:
						rectTransform.anchorMin = new Vector2(1f, 0f);
						rectTransform.anchorMax = new Vector2(1f, 0f);
						rectTransform.sizeDelta = new Vector2(16f, 16f);
						break;
				}
				
				rectTransform.anchoredPosition = Vector2.zero;
			}
		}
	
		private void OnDrag(BaseEventData data) {
			PointerEventData pointerEventData = (PointerEventData)data;
			Target.Translate(pointerEventData.delta);
		}
	}
}