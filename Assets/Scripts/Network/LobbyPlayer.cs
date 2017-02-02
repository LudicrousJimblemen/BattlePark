using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyPlayer : NetworkLobbyPlayer {
	
	[SyncVar]
	public string Username;

	//insert stuff to pass to the real player here

	/// <summary>
	/// Toggles the ready state of the player
	/// </summary>
	/// <returns>
	/// Returns the ready state after toggling, false if it's not the local player
	/// </returns>
	public bool ToggleReady () {
		if (!isLocalPlayer) return false;
		readyToBegin = !readyToBegin;
		if (readyToBegin) 
			SendReadyToBeginMessage ();
		else 
			SendNotReadyToBeginMessage ();
		return readyToBegin;
	}
	public override void OnStartLocalPlayer () {
		if (!isLocalPlayer) return;
		Client.Instance.localPlayer = this;
		LobbyGUI.Instance.LobbyReadyButton.interactable = true;
		CmdUpdateInfo(LobbyGUI.Instance.Username);
	}

	//use for other stuff to update
	//right now only updates username
	[Command] 
	public void CmdUpdateInfo (string username) {
		Username = username;
	}

	[ClientRpc]
	public void RpcPrepareReady () {
		LobbyGUI.Instance.FadeToWhite ();
	}
}