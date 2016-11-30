using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BattlePark.Core;

namespace BattlePark.Menu {
	public class LobbyGUI : MonoBehaviour {
		public Image Fade;
	
		public Text UsersPanel;
		public Text ChatTextPanel;
	
		public Button LeaveButton;
		public Button ReadyButton;

		public InputField ChatInputField;

		private Client client;
	
		private bool inAnimation;

		public IEnumerator LoadTitleScreen() {
			inAnimation = true;

			//client.Close();

			for (float i = 0; i < 60; i++) {
				Fade.color = Color.Lerp(new Color(1f, 1f, 1f, 0), Color.white, Mathf.SmoothStep(0f, 1f, i / 40f));
				yield return null;
			}

			UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
		}

		private void Awake() {
			ChatInputField.onEndEdit.AddListener(Chat);
		
			LeaveButton.onClick.AddListener(() => StartCoroutine(LoadTitleScreen()));
			ReadyButton.onClick.AddListener(Ready);

			client = FindObjectOfType<Client>();
			
			client.CreateListener<ServerUserUpdateNetMessage>(OnServerUserUpdate);
			client.CreateListener<ServerUserJoinNetMessage>(OnServerUserJoin);
			client.CreateListener<ServerChatNetMessage>(OnServerChat);
			
			client.SendMessage<ClientRequestPlayersNetMessage>(new ClientRequestPlayersNetMessage());

			StartCoroutine(FadeGraphic(Fade, 0, 60f, Color.black, Color.clear));
		}
	
		private void Update() {
			Fade.raycastTarget = inAnimation;
		}
	
		private IEnumerator FadeGraphic(Graphic graphic, float delay, float duration, Color fromColor, Color toColor) {
			for (int i = 0; i < duration + delay; i++) {
				inAnimation = true;
				graphic.color = Color.Lerp(fromColor, toColor, Mathf.SmoothStep(0f, 1f, (i - delay) / duration));
				yield return null;
			}
			inAnimation = false;
		}
		
		private void Ready() {
			ReadyButton.interactable = false;
			
			client.SendMessage<ClientReadyNetMessage>(new ClientReadyNetMessage());
		}
		
		private void Chat(string e) {
			if (Input.GetKey(KeyCode.Return)) {
				client.SendMessage<ClientChatNetMessage>(new ClientChatNetMessage { Message = ChatInputField.text });
				ChatInputField.text = String.Empty;
			} else {
				return;
			}
		}
		
		private void OnServerUserUpdate(ServerUserUpdateNetMessage message) {
			string output = String.Empty;
			
			foreach (var user in message.Users) {
				output += String.Format("<color=#{0}>{1}</color>\n", user.Ready? "ffffffff" : "ccccccff", user.Username);
			}
			
			UsersPanel.text = output;
		}
		
		private void OnServerUserJoin(ServerUserJoinNetMessage message) {
			ChatTextPanel.text += "\n" + String.Format(LanguageManager.GetString("chat.userJoined"), message.NewUser.Username);
		}
		
		private void OnServerChat(ServerChatNetMessage message) {
			ChatTextPanel.text += "\n" + String.Format(LanguageManager.GetString("chat.chatMessage"), message.Sender.Username, message.Message);
		}
	}
}