using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Linq;

public class LobbyPlayer : NetworkLobbyPlayer {
	public string Username = String.Empty;
	
	public override void OnClientEnterLobby() {
		base.OnClientEnterLobby();

		LobbyManager.Instance.PlayerCount += 1;

		if (!LobbyManager.Instance.GUI.lobbyPlayers.Contains(this)) {
			LobbyManager.Instance.GUI.lobbyPlayers.Add(this);
		}

		if (!isLocalPlayer) {
			SetupOtherPlayer();
		}
	}

	void SetupOtherPlayer() {
		OnClientReady(false);
	}

	public void OnDestroy() {
		LobbyManager.Instance.GUI.lobbyPlayers.Remove(this);
		LobbyManager.Instance.PlayerCount -= 1;
	}
}
