using System;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using BattlePark.Core;

namespace BattlePark {
	public class GameClient : MonoBehaviour {
		private Client messenger = new Client();

		public List<GameUser> Users = new List<GameUser>();

		private void Start() {
			DontDestroyOnLoad(gameObject);
		}

		private void Update() {
			messenger.UpdateListeners();
		}

		public void Close() {
			messenger.Close();
		}
		
		public void Join(string username, string ip, int port) {
			messenger.JoinOnlineGame(username, GameConfig.Version, ip, port);
		}
	}
}