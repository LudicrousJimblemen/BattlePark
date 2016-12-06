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
		
		private void AddListeners() {
			client.CreateListener<ServerUserUpdateNetMessage>(OnServerUserUpdate);
			client.CreateListener<ServerUserJoinNetMessage>(OnServerUserJoin);
			client.CreateListener<ServerUserLeaveNetMessage>(OnServerUserLeave);
			client.CreateListener<ServerChatNetMessage>(OnServerChat);
			client.CreateListener<ServerInitializeGameNetMessage>(OnServerInitializeGame);
		}
		
		private void RemoveListeners() {
			client.RemoveListener<ServerUserUpdateNetMessage>(OnServerUserUpdate);
			client.RemoveListener<ServerUserJoinNetMessage>(OnServerUserJoin);
			client.RemoveListener<ServerUserLeaveNetMessage>(OnServerUserLeave);
			client.RemoveListener<ServerChatNetMessage>(OnServerChat);
			client.RemoveListener<ServerInitializeGameNetMessage>(OnServerInitializeGame);
		}
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			GUI.ChatInputField.onEndEdit.AddListener(Chat);
		
			GUI.LeaveButton.onClick.AddListener(Leave);
			GUI.ReadyButton.onClick.AddListener(Ready);
			
			AddListeners();
			
			client.SendMessage<ClientRequestPlayersNetMessage>(new ClientRequestPlayersNetMessage());
		}
		
		private void OnApplicationQuit() {
			client.SendMessage<ClientDisconnectNetMessage>(new ClientDisconnectNetMessage());
			client.Close();
		}
		
		private void Leave() {
			client.SendMessage<ClientDisconnectNetMessage>(new ClientDisconnectNetMessage());
			
			RemoveListeners();
			
			client.Close();
			
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, new Color(1f, 1f, 1f, 0f), Color.white, false, () => {
				UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
			}));
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
		
		private void OnServerUserLeave(ServerUserLeaveNetMessage message) {
			GUI.ChatTextPanel.text += "\n" + String.Format(LanguageManager.GetString("chat.userLeft"), message.User.Username);
		}
		
		private void OnServerChat(ServerChatNetMessage message) {
			GUI.ChatTextPanel.text += "\n" + String.Format(LanguageManager.GetString("chat.chatMessage"), message.Sender.Username, message.Message);
		}
		
		private void OnServerInitializeGame(ServerInitializeGameNetMessage message) {
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, Color.clear, Color.black, false, () => {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
			}));
			
			RemoveListeners();
		}
	}
}