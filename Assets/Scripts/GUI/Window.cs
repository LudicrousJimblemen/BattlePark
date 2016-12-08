using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattlePark.Menu {
	[RequireComponent(typeof(EventTrigger))]
	public class Window : MonoBehaviour {
		public GameObject ResizerPrefab;
		
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
			
			for (int i = 0; i < 4; i++) {
				GameObject resizer = (GameObject)Instantiate(ResizerPrefab, Vector3.zero, Quaternion.identity, transform.parent);
				Image image = resizer.GetComponent<Image>();
				image.color = Random.ColorHSV(0, 1f, 1f, 1f, 1f, 1f, 0.75f, 0.75f);
				
				WindowResizer windowResizer = resizer.GetComponent<WindowResizer>();
				windowResizer.Type = (WindowResizerType)i;
				windowResizer.Target = Target;
				
				RectTransform rectTransform = resizer.GetComponent<RectTransform>();
				
				switch ((WindowResizerType)i) {
					case WindowResizerType.Top:
						rectTransform.anchorMin = new Vector2(0, 1f);
						rectTransform.anchorMax = new Vector2(1f, 1f);
						rectTransform.sizeDelta = new Vector2(-16f, 16f);
						break;
						
					case WindowResizerType.Bottom:
						rectTransform.anchorMin = new Vector2(0, 0f);
						rectTransform.anchorMax = new Vector2(1f, 0f);
						rectTransform.sizeDelta = new Vector2(-16f, 16f);
						break;
						
					case WindowResizerType.Left:
						rectTransform.anchorMin = new Vector2(0, 0f);
						rectTransform.anchorMax = new Vector2(0f, 1f);
						rectTransform.sizeDelta = new Vector2(16f, -16f);
						break;
						
					case WindowResizerType.Right:
						rectTransform.anchorMin = new Vector2(1, 0f);
						rectTransform.anchorMax = new Vector2(1f, 1f);
						rectTransform.sizeDelta = new Vector2(16f, -16f);
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