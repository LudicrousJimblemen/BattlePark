using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;


public class LobbyManager : NetworkLobbyManager {
	public static LobbyManager Instance;

	//Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
	//of players, so that even client know how many player there is.
	[HideInInspector]
	public int _playerNumber = 0;

	//used to disconnect a client properly when exiting the matchmaker
	[HideInInspector]
	public bool _isMatchmaking = false;

	public LobbyGUI GUI;
    
	protected bool _disconnectServer = false;
	protected ulong _currentMatchID;
	protected LobbyHook _lobbyHooks;

	private void Start() {
		Instance = this;
		_lobbyHooks = GetComponent<LobbyHook>();

		DontDestroyOnLoad(gameObject);
	}
    
	public void CreateRoom(string roomName) {
		StartMatchMaker();
		matchMaker.CreateMatch(
			roomName,
			(uint)maxPlayers,
			true,
			"", "", "", 0, 0,
			OnMatchCreate
		);
    	
		_isMatchmaking = true;
	}

	// ----------------- Server management

	public void AddLocalPlayer() {
		TryToAddPlayer();
	}

	public void RemovePlayer(LobbyPlayer player) {
		player.RemovePlayer();
	}

	class KickMsg : MessageBase {
		public const short Code = MsgType.Highest + 1;

	}
	public void KickPlayer(NetworkConnection conn) {
		conn.Send(KickMsg.Code, new KickMsg());
	}

	public void KickMessageHandler(NetworkMessage netMsg) {
		//got kickt
		netMsg.conn.Disconnect();
	}

	public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
		base.OnMatchCreate(success, extendedInfo, matchInfo);
		_currentMatchID = (System.UInt64)matchInfo.networkId;
	}

	public override void OnDestroyMatch(bool success, string extendedInfo) {
		base.OnDestroyMatch(success, extendedInfo);
		if (_disconnectServer) {
			StopMatchMaker();
			StopHost();
		}
	}

	//allow to handle the (+) button to add/remove player
	public void OnPlayersNumberModified(int count) {
		_playerNumber += count;

		int localPlayerCount = 0;
		foreach (PlayerController p in ClientScene.localPlayers)
			localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;
	}

	// ----------------- Server callbacks ------------------

	//we want to disable the button JOIN if we don't have enough player
	//But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
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

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
		//This hook allows you to apply state data from the lobby-player to the game-player
		//just subclass "LobbyHook" and add it to the lobby object.

		if (_lobbyHooks)
			_lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

		return true;
	}

	// --- Countdown management

	public override void OnLobbyServerPlayersReady() {
		bool allready = true;
		for (int i = 0; i < lobbySlots.Length; ++i) {
			if (lobbySlots[i] != null)
				allready &= lobbySlots[i].readyToBegin;
		}

		if (allready) {
			ServerChangeScene(playScene);
		}
	}

	// ----------------- Client callbacks ------------------

	public override void OnClientConnect(NetworkConnection conn) {
		base.OnClientConnect(conn);

		conn.RegisterHandler(KickMsg.Code, KickMessageHandler);

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
