using System;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;

public class GameClient : MonoBehaviour {
	private Client messenger = new Client();

	public List<GameUser> Users = new List<GameUser>();

	public void Close() {
		messenger.Close();
	}

	private void Start() {
		DontDestroyOnLoad(gameObject);

		messenger.JoinOnlineGame(NetworkManager.Ip, NetworkManager.Port);
	}

	private void Update() {
		messenger.UpdateListeners();
	}
}