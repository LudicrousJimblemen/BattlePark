using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BattlePark;

public class PropertyWindow : MonoBehaviour {
	public void Initialize (int MinimumSizeX, string Name, GridObjectProperty[] Properties) {
		foreach (FlexibleResizeHandler resizeHandler in transform.FindChild ("Handlers").GetComponentsInChildren<FlexibleResizeHandler>()) {
			resizeHandler.MinimumDimensions = new Vector2 (MinimumSizeX, 100);
			resizeHandler.MaximumDimensions = new Vector2 (Screen.width - 20, Screen.height - 20);
		}
		foreach (GridObjectProperty property in Properties) {
			AddProperty(property);
		}
		SetName(Name);
		transform.localScale = Vector3.one;
	}

	void Update () {
		transform.FindChild("Handlers").gameObject.SetActive(transform.GetSiblingIndex() == transform.parent.childCount - 1);
	}

	public void OnFocus() {
		transform.FindChild("Focus Object").gameObject.SetActive(false);
		transform.SetAsLastSibling();
	}

	public void OnDefocus() {
		transform.FindChild("Focus Object").gameObject.SetActive(true);
	}

	public void OnClose () {
		FindObjectOfType<WindowManager>().CloseWindow(this);
		Destroy(gameObject);
	}

	public void SetName (string Name) {
		transform.GetComponentInChildren<FlexibleDraggableObject>().GetComponentInChildren<Text>().text = Name;
	}

	public void AddProperty (GridObjectProperty Property) {
		Transform content = transform.FindChild("Content");
		GameObject propObj = (GameObject)Instantiate(new GameObject(Property.Name));
	}
}
