using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class LobbyManager : NetworkLobbyManager {
	public static LobbyManager Instance;
	
	public LobbyGUI GUI;

	public ulong CurrentMatchID { get; set; }
	public bool IsMatchmaking { get; set; }
	public int PlayerCount { get; set; }

	private void Start() {
		Instance = this;

		DontDestroyOnLoad(gameObject);
	}
    
	public void CreateRoom(string roomName) {
		StartMatchMaker();
		matchMaker.CreateMatch(
			roomName,
			(uint)maxPlayers,
			true,
			String.Empty, String.Empty, String.Empty, 0, 0,
			OnMatchCreate
		);
    	
		IsMatchmaking = true;
		
		GUI.AnimatePanel(GUI.LobbyPanel, 1);
	}

	public void AddLocalPlayer() {
		TryToAddPlayer();
	}

	public void RemovePlayer(LobbyPlayer player) {
		player.RemovePlayer();
	}

	/*
	class KickMsg : MessageBase {
		public const short Code = MsgType.Highest + 1;
	}
	*/

	public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
		base.OnMatchCreate(success, extendedInfo, matchInfo);
		CurrentMatchID = (ulong)matchInfo.networkId;
	}

	public override void OnDestroyMatch(bool success, string extendedInfo) {
		base.OnDestroyMatch(success, extendedInfo);
		// AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA???
	}

	// ----------------- Server callbacks ------------------

	public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId) {
		GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

		LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();

		for (int i = 0; i < lobbySlots.Length; ++i) {
			LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

			if (p != null) {
				//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaa
			}
		}

		return obj;
	}

	public override void OnLobbyServerPlayersReady() {
		bool allReady = true;
		for (int i = 0; i < lobbySlots.Length; ++i) {
			if (lobbySlots[i] != null) {
				allReady &= lobbySlots[i].readyToBegin;
			}
		}

		if (allReady) {
			ServerChangeScene(playScene);
		}
	}

	// ----------------- Client callbacks ------------------

	public override void OnClientConnect(NetworkConnection conn) {
		base.OnClientConnect(conn);

		//conn.RegisterHandler(KickMsg.Code, KickMessageHandler);

		if (!NetworkServer.active) {
			GUI.AnimatePanel(GUI.LobbyPanel, 1);
		}
	}


	public override void OnClientDisconnect(NetworkConnection conn) {
		base.OnClientDisconnect(conn);
		GUI.AnimatePanel(GUI.FindGamePanel, -1);
	}

	public override void OnClientError(NetworkConnection conn, int errorCode) {
		GUI.AnimatePanel(GUI.MainPanel, -1);
		Debug.LogError("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()));
	}
}
