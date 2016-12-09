using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class FlexibleDraggableObject : MonoBehaviour
{
    public GameObject Target;
    private RectTransform rectTransform;
    private EventTrigger _eventTrigger;
    
    void Start ()
    {
    	rectTransform = Target.GetComponent<RectTransform>();
        _eventTrigger = GetComponent<EventTrigger>();
        _eventTrigger.AddEventTrigger(OnDrag, EventTriggerType.Drag);
		_eventTrigger.AddEventTrigger(OnClicked,EventTriggerType.PointerClick);
	}

	private void OnClicked(BaseEventData data) {
		transform.parent.SetAsLastSibling();
	}

	void OnDrag(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData) data;
        Target.transform.Translate(ped.delta);
		Target.transform.position = new Vector3(Mathf.Clamp(rectTransform.position.x,rectTransform.rect.width / 2,Screen.width - rectTransform.rect.width / 2),
									Mathf.Clamp(rectTransform.position.y,rectTransform.rect.height / 2,Screen.height - rectTransform.rect.height / 2),
									0);
		transform.parent.SetAsLastSibling();
	}
}