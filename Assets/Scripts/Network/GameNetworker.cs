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
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			client.CreateListener<ServerStartGameNetMessage>(OnServerStartGame);
			
			client.SendMessage<ClientGameReadyNetMessage>(new ClientGameReadyNetMessage());
		}
		
		private void OnServerStartGame(ServerStartGameNetMessage message) {
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, Color.black, Color.clear));
		}
	}
}