using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyPlayer : NetworkLobbyPlayer {
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
		print ("wow");
		Client.Instance.localPlayer = this;
	}
	public override void OnClientExitLobby () {
		//base.OnClientExitLobby ();
		//print ("exit ass");
		//SendNotReadyToBeginMessage ();
	}
}