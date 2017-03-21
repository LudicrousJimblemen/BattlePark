using UnityEngine;
using UnityEngine.Networking;
using BattlePark.GUI;

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
			LogChat(string.Format(LanguageManager.GetString("chat.userReadied"), Username), LogType.Ready);
		} else {
			SendNotReadyToBeginMessage();
			LogChat(string.Format(LanguageManager.GetString("chat.userUnreadied"), Username), LogType.Unready);
		}
		return readyToBegin;
	}

	/// <summary>
	/// Chats.
	/// </summary>
	public void Chat(string message) {
		CmdSendChat(string.Format("<<color=#e0e0e0ff>{0}</color>> ", Username) + message);
	}

	public enum LogType {
		Join,
		Leave,
		Ready,
		Unready
	}
	public void LogChat(string message, LogType logType) {
		string color; // a hex color, or a predefined color
		switch (logType) {
			case LogType.Join:
				color = "lightblue";
				break;
			case LogType.Leave:
				color = "lightblue";
				break;
			case LogType.Ready:
				color = "orange";
				break;
			case LogType.Unready:
				color = "orange";
				break;
			default:
				color = "white";
				break;
		}
		CmdSendChat(string.Format("<color={0}>{1}</color>", color, message));
	}

	public override void OnStartLocalPlayer() {
		if (!isLocalPlayer) {
			return;
		} else {
			LobbyGUI.Instance.LobbyReadyButton.interactable = true;
		}
		Client.Instance.localPlayer = this;
		CmdUpdateInfo(LobbyGUI.Instance.Username);
	}

	[Command]
	public void CmdSendChat(string message) {
		RpcSendChat(message);
	}

	[ClientRpc]
	public void RpcSendChat(string message) {
		LobbyGUI.Instance.AddChat(message);
	}

	// use for other stuff to update
	// right now only updates username
	[Command]
	public void CmdUpdateInfo(string username) {
		Username = username;
		LogChat(string.Format(LanguageManager.GetString("chat.userJoined"), Username), LogType.Join);
	}

	[ClientRpc]
	public void RpcPrepareReady() {
		// TODO remove this garbage
		LobbyGUI.Instance.FadeToWhite();
	}
}