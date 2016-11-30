using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BattlePark.Core;

namespace BattlePark.Menu {
	public class LobbyNetworker : MonoBehaviour {
		public LobbyGUI GUI;
		
		private Client client;
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			GUI.ChatInputField.onEndEdit.AddListener(Chat);
		
			//GUI.LeaveButton.onClick.AddListener(() => StartCoroutine(LoadTitleScreen()));
			GUI.ReadyButton.onClick.AddListener(Ready);

			client = FindObjectOfType<Client>();
			
			client.CreateListener<ServerUserUpdateNetMessage>(OnServerUserUpdate);
			client.CreateListener<ServerUserJoinNetMessage>(OnServerUserJoin);
			client.CreateListener<ServerChatNetMessage>(OnServerChat);
			client.CreateListener<ServerInitializeGameNetMessage>(OnServerInitializeGame);
			
			client.SendMessage<ClientRequestPlayersNetMessage>(new ClientRequestPlayersNetMessage());
		}
		
		private void Ready() {
			GUI.ReadyButton.interactable = false;
			
			client.SendMessage<ClientLobbyReadyNetMessage>(new ClientLobbyReadyNetMessage());
		}
		
		private void Chat(string e) {
			if (Input.GetKey(KeyCode.Return)) {
				client.SendMessage<ClientChatNetMessage>(new ClientChatNetMessage { Message = GUI.ChatInputField.text });
				GUI.ChatInputField.text = String.Empty;
			} else {
				return;
			}
		}
		
		private void OnServerUserUpdate(ServerUserUpdateNetMessage message) {
			string output = String.Empty;
			
			foreach (var user in message.Users) {
				output += String.Format("<color=#{0}>{1}</color>\n", user.LobbyReady? "ffffffff" : "ccccccff", user.Username);
			}
			
			GUI.UsersPanel.text = output;
		}
		
		private void OnServerUserJoin(ServerUserJoinNetMessage message) {
			GUI.ChatTextPanel.text += "\n" + String.Format(LanguageManager.GetString("chat.userJoined"), message.NewUser.Username);
		}
		
		private void OnServerChat(ServerChatNetMessage message) {
			GUI.ChatTextPanel.text += "\n" + String.Format(LanguageManager.GetString("chat.chatMessage"), message.Sender.Username, message.Message);
		}
		
		private void OnServerInitializeGame(ServerInitializeGameNetMessage message) {
			client.RemoveListener<ServerUserUpdateNetMessage>(OnServerUserUpdate);
			client.RemoveListener<ServerUserJoinNetMessage>(OnServerUserJoin);
			client.RemoveListener<ServerChatNetMessage>(OnServerChat);
			client.RemoveListener<ServerInitializeGameNetMessage>(OnServerInitializeGame);
			
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, Color.clear, Color.black, false, () => {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
			}));
		}
	}
}