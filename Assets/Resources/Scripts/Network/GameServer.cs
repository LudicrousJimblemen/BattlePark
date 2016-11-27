using System;
using UnityEngine;
using Lidgren.Network;

public class GameServer : MonoBehaviour {
	private Server messenger = new Server();

	private void Start() {
		DontDestroyOnLoad(gameObject);

		messenger.StartServer(NetworkManager.Port);
	}

	private void Update() {
		messenger.UpdateListeners();
	}
}