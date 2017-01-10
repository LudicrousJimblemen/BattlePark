using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Linq;

public class LobbyPlayer : NetworkLobbyPlayer {
	public string Username = String.Empty;

	public LobbyPlayer(string username) {
		Username = username;
	}
	
	public override void OnClientEnterLobby() {
		base.OnClientEnterLobby();

		if (LobbyManager.Instance != null)
			LobbyManager.Instance.OnPlayersNumberModified(1);

		LobbyPlayerList.Instance.AddPlayer(this);
		LobbyPlayerList.Instance.DisplayDirectServerWarning(isServer && LobbyManager.Instance.matchMaker == null);

		if (isLocalPlayer) {
			SetupLocalPlayer();
		} else {
			SetupOtherPlayer();
		}
	}

	public override void OnStartAuthority() {
		base.OnStartAuthority();

		SetupLocalPlayer();
	}

	void SetupOtherPlayer() {
		OnClientReady(false);
	}

	void SetupLocalPlayer() {
		//when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
		//the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
		if (LobbyManager.Instance != null)
			LobbyManager.Instance.OnPlayersNumberModified(0);
	}

	//===== UI Handler

	public void OnRemovePlayerClick() {
		if (isLocalPlayer) {
			RemovePlayer();
		} else if (isServer)
			LobbyManager.Instance.KickPlayer(connectionToClient);
                
	}

	public void OnDestroy() {
		LobbyPlayerList.Instance.RemovePlayer(this);
		if (LobbyManager.Instance != null)
			LobbyManager.Instance.OnPlayersNumberModified(-1);
	}
}
