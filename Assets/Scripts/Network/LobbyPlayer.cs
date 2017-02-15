using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyPlayer : NetworkLobbyPlayer {
	[SyncVar]
	public string Username;

	// insert stuff to pass to the real player here

	/// <summary>
	/// Toggles the ready state of the player.
	/// </summary>
	/// <returns>
	/// The ready state after toggling, or false if it's not the local player.
	/// </returns>
	public bool ToggleReady() {
		if (!isLocalPlayer) {
			return false;
		}
		
		readyToBegin = !readyToBegin;
		if (readyToBegin) {
			SendReadyToBeginMessage();
		} else {
			SendNotReadyToBeginMessage();
		}
		return readyToBegin;
	}
	
	/// <summary>
	/// Chats.
	/// </summary>
	public void Chat(string message) {
		CmdSendChat(Username, message);
	}
	
	public override void OnStartLocalPlayer() {
		if (!isLocalPlayer) {
			return;
		}
		Client.Instance.localPlayer = this;
		LobbyGUI.Instance.LobbyReadyButton.interactable = true;
		CmdUpdateInfo(LobbyGUI.Instance.Username);
	}
	
	[Command]
	public void CmdSendChat(string username, string message) {
		RpcSendChat(username, message);
	}
	
	[ClientRpc]
	public void RpcSendChat(string username, string message) {
		LobbyGUI.Instance.AddChat(username, message);
	}

	// use for other stuff to update
	// right now only updates username
	[Command] 
	public void CmdUpdateInfo(string username) {
		Username = username;
	}

	[ClientRpc]
	public void RpcPrepareReady() {
		// TODO remove this garbage
		LobbyGUI.Instance.FadeToWhite();
	}
}