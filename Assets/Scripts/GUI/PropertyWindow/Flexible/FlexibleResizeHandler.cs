using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public enum HandlerType
{
    TopRight,
    Right,
    BottomRight,
    Bottom,
    BottomLeft,
    Left,
    TopLeft,
    Top
}

[RequireComponent(typeof(EventTrigger))]
public class FlexibleResizeHandler : MonoBehaviour
{
    public HandlerType Type;
    public RectTransform Target;
    public Vector2 MinimumDimensions = new Vector2(50, 50);
    public Vector2 MaximumDimensions = new Vector2(800, 800);
    
    private EventTrigger _eventTrigger;
    
	void Start ()
	{
	    _eventTrigger = GetComponent<EventTrigger>();
        _eventTrigger.AddEventTrigger(OnDrag, EventTriggerType.Drag);
	}

    void OnDrag(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData) data;
        RectTransform.Edge? horizontalEdge = null;
        RectTransform.Edge? verticalEdge = null;

        switch (Type)
        {
            case HandlerType.TopRight:
                horizontalEdge = RectTransform.Edge.Left;
                verticalEdge = RectTransform.Edge.Bottom;
                break;
            case HandlerType.Right:
                horizontalEdge = RectTransform.Edge.Left;
                break;
            case HandlerType.BottomRight:
                horizontalEdge = RectTransform.Edge.Left;
                verticalEdge = RectTransform.Edge.Top;
                break;
            case HandlerType.Bottom:
                verticalEdge = RectTransform.Edge.Top;
                break;
            case HandlerType.BottomLeft:
                horizontalEdge = RectTransform.Edge.Right;
                verticalEdge = RectTransform.Edge.Top;
                break;
            case HandlerType.Left:
                horizontalEdge = RectTransform.Edge.Right;
                break;
            case HandlerType.TopLeft:
                horizontalEdge = RectTransform.Edge.Right;
                verticalEdge = RectTransform.Edge.Bottom;
                break;
            case HandlerType.Top:
                verticalEdge = RectTransform.Edge.Bottom;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (horizontalEdge != null)
        {
        	float width;
        	if (horizontalEdge == RectTransform.Edge.Right){
				//width = Target.rect.width - ped.delta.x;
				width = Target.position.x + Target.pivot.x * Target.rect.width - ped.position.x;
            	width = Mathf.Clamp(width, MinimumDimensions.x, MaximumDimensions.x);
				width = Mathf.Clamp(width, MinimumDimensions.x,Target.position.x + Target.pivot.x * Target.rect.width);
				Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)horizontalEdge, 
                    Screen.width - Target.position.x - Target.pivot.x * Target.rect.width, 
                    width);
        	} 
        	else 
        	{
				//width = Target.rect.width + ped.delta.x;
				width = ped.position.x - Target.position.x + Target.pivot.x * Target.rect.width;
				width = Mathf.Clamp(width, MinimumDimensions.x, MaximumDimensions.x);
				width = Mathf.Clamp(width, MinimumDimensions.x,Screen.width - Target.position.x + Target.pivot.x * Target.rect.width);
                Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)horizontalEdge, 
                    Target.position.x - Target.pivot.x * Target.rect.width, 
                    width);
        	}
        }
        if (verticalEdge != null)
        {
        	float height;
        	if (verticalEdge == RectTransform.Edge.Top) {
				//height = Target.rect.height - ped.delta.y;
				height = Target.position.y + Target.pivot.y * Target.rect.height - ped.position.y;
				height = Mathf.Clamp(height, MinimumDimensions.y, MaximumDimensions.y);
				height = Mathf.Clamp(height, MinimumDimensions.y,Target.position.y + Target.pivot.y * Target.rect.height);
				Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)verticalEdge, 
                    Screen.height - Target.position.y - Target.pivot.y * Target.rect.height, 
                    height);
            	
        	}
        	else
        	{
				//height = Target.rect.height + ped.delta.y;
				height = ped.position.y - Target.position.y + Target.pivot.y * Target.rect.height;
				height = Mathf.Clamp(height, MinimumDimensions.y, MaximumDimensions.y);
				height = Mathf.Clamp(height, MinimumDimensions.y,Screen.height - Target.position.y + Target.pivot.y * Target.rect.height);
				Target.SetInsetAndSizeFromParentEdge((RectTransform.Edge)verticalEdge, 
                    Target.position.y - Target.pivot.y * Target.rect.height, 
                    height);
        	}
        }
		
    }
}
