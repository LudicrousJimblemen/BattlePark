using System;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour {
	public static GameGUI Instance;

	public Text Money;

	public Window PlaceholderWindow;
	public FollowCamera PlaceholderCamera;
	
	public Button TogglePlaceholderButton;
	
	private void Awake() {
		Instance = this;
    }
	
	private void Update() {
		Money.text = Player.Local.Money.ToString(LanguageManager.GetString("game.gui.numericCurrencySmall"));
	}

	private void Start() {
		PlaceholderCamera = Instantiate(PlaceholderCamera,Vector3.zero,Quaternion.identity);
		PlaceholderCamera.Output = new RenderTexture(190,190,16);

		PlaceholderWindow = Instantiate(PlaceholderWindow,Vector3.zero,Quaternion.identity,WindowManager.Instance.transform);
		UpdatePlaceholderWindow(null,null);
		PlaceholderWindow.Type = WindowType.Placeholder;
		PlaceholderWindow.gameObject.SetActive(false);

		WindowManager.Instance.Windows.Add(PlaceholderWindow);
		TogglePlaceholderButton.onClick.AddListener(TogglePlaceholderWindow);
	}

	// placeholderObject null means no placing
	// target null means no placing
	public void UpdatePlaceholderWindow(GridObject placeholderObject, Transform target) {
		PlaceholderWindow.TitleText.text = String.Format(LanguageManager.GetString("game.gui.placing"),placeholderObject == null ? "None" : LanguageManager.GetString(placeholderObject.ProperString));
		PlaceholderWindow.GetComponentInChildren<RawImage>().texture = placeholderObject == null ? null : PlaceholderCamera.Output;

		PlaceholderCamera.gameObject.SetActive(target != null);
		if(target != null) {
			PlaceholderCamera.Target = target;
		}
    }

	public void TogglePlaceholderWindow() {
		PlaceholderWindow.gameObject.SetActive(!PlaceholderWindow.gameObject.activeInHierarchy);
	}
}
