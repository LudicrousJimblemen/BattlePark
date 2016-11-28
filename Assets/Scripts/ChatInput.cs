using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattlePark {
	public class ChatInput : InputField {
		public Text ChatText;
	
		private CameraPan camera;
	
		private bool submitted;
	
		void Start() {
			ChatText = FindObjectsOfType<Text>().First(x => x.name == "ChatText");
		
			camera = FindObjectOfType<CameraPan>();
		}
	
		void Update() {
			camera.Enabled = !isFocused;
		
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
				//SEND THE CHAT!!!!!!!!!!
				text = String.Empty;
			}
			submitted = false;
		}
	}
}