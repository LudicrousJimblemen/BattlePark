using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatInput : InputField {
	public Text ChatText;
	
	private Player player;
	private Client client;
	private CameraPan camera;
	
	private bool submitted;
	
	void Start() {
		ChatText = FindObjectsOfType<Text>().First(x => x.name == "ChatText");

		client = FindObjectOfType<Client> ();
		player = FindObjectOfType<Player>();
		camera = FindObjectOfType<CameraPan>();
	}
	
	void Update() {
		camera.Enabled = !isFocused;
		player.Enabled = !isFocused;
		
		if (Input.GetKeyUp(KeyCode.Slash)) {
			EventSystem.current.SetSelectedGameObject(gameObject, null);
			OnPointerClick(new PointerEventData(EventSystem.current));
		}
		if (Input.GetKeyUp(KeyCode.Return)) {
			if (!String.IsNullOrEmpty(text)) {
				submitted = true;
			} else {
				submitted = false;
			}
			OnDeselect(new BaseEventData(EventSystem.current));
		}
		if (Input.GetKeyUp(KeyCode.Escape)) {
			submitted = false;
			OnDeselect(new BaseEventData(EventSystem.current));
		}
	}
	
	public override void OnDeselect(BaseEventData eventData) {
		interactable = false;
		base.OnDeselect(eventData);
		interactable = true;
		
		if (submitted) {
			client.NetworkClient.Send(ChatNetMessage.Code, new ChatNetMessage {
				Message = text
			});
			text = String.Empty;
		}
		submitted = false;
	}
}