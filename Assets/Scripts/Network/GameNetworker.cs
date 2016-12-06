using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BattlePark.Core;

namespace BattlePark.Menu {
	public class GameNetworker : MonoBehaviour {
		public GameGUI GUI;
		
		private Client client;
		
		private void AddListeners() {
			client.CreateListener<ServerStartGameNetMessage>(OnServerStartGame);
			client.CreateListener<ServerEndGameNetMessage>(OnServerEndGame);
		}
		
		private void RemoveListeners() {
			client.RemoveListener<ServerStartGameNetMessage>(OnServerStartGame);
			client.RemoveListener<ServerEndGameNetMessage>(OnServerEndGame);
		}
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			AddListeners();
			
			client.SendMessage<ClientGameReadyNetMessage>(new ClientGameReadyNetMessage());
		}
		
		private void OnApplicationQuit() {
			client.SendMessage<ClientDisconnectNetMessage>(new ClientDisconnectNetMessage());
			client.Close();
		}
		
		private void OnServerStartGame(ServerStartGameNetMessage message) {
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, Color.black, Color.clear));
		}
		
		private void OnServerEndGame(ServerEndGameNetMessage message) {
			client.SendMessage<ClientDisconnectNetMessage>(new ClientDisconnectNetMessage());
			
			RemoveListeners();
			
			client.Close();
			
			
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, Color.clear, Color.white, false, () => {
				UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
			}));
		}
	}
}