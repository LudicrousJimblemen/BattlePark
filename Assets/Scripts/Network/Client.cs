using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class Client : NetworkLobbyManager {
	public static Client Instance;
	
	[HideInInspector]
	public bool IsHost;
	
	public LobbyPlayer localPlayer;

	private short highestPlayerNum;
	
	private void Awake() {
		if (FindObjectsOfType<Client>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad(gameObject);
		Instance = this;
		//print (NetworkManager.singleton == null);
		//StartCoroutine(WaitForNetworkManager());
	}
	
	private IEnumerator WaitForNetworkManager() {
		while (NetworkManager.singleton == null) {
			yield return new WaitForEndOfFrame();
		}
		//NetworkManager.singleton.StartMatchMaker();
	}
	
	public void CreateMatch(string roomName, uint size, NetworkMatch.DataResponseDelegate<MatchInfo> callback) {
		base.matchMaker.CreateMatch(
			roomName,
			size,
			true,
			String.Empty,
			String.Empty,
			String.Empty,
			0,
			0,
			(success, extendedInfo, matchInfo) => {
				IsHost = success;
				if (success) {
					NetworkServer.Listen(matchInfo, 6666);
					StartHost(matchInfo);
				}
				callback(success, extendedInfo, matchInfo);
			});
	}
	
	public void ListMatches(int maxResults, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> callback) {
		matchMaker.ListMatches(
			0,
			maxResults,
			String.Empty,
			true,
			0,
			0,
			callback);
	}
	
	public void JoinMatch(NetworkID id, NetworkMatch.DataResponseDelegate<MatchInfo> callback) {
		matchMaker.JoinMatch(
			id,
			String.Empty,
			String.Empty,
			String.Empty,
			0,
			0,
			(success, extendedInfo, matchInfo) => {
				IsHost = false;
				if (success) {
					StartClient(matchInfo);
				}
				callback(success, extendedInfo, matchInfo);
			});
	}
	
	private void DropConnection() {
		StopMatchMaker();
		if (IsHost) {
			StopHost();
		} else {
			StopClient();
		}
		
		if (localPlayer != null) {
			localPlayer.RemovePlayer();
			Destroy(localPlayer.gameObject);
		}
		LobbyGUI.Instance.LoadMain();
	}
	
	private IEnumerator StartGame() {
		for (int i = 0; i < lobbySlots.Length; i++) {
			if (lobbySlots[i] == null)
				break;
			((LobbyPlayer)lobbySlots[i]).RpcPrepareReady();
		}
		yield return new WaitForSeconds(2.0f);
		ServerChangeScene(playScene);
	}
	
	public override void OnLobbyServerPlayersReady() {
		print("mate, we're shipshape");
		StartCoroutine(StartGame());
	}
	
	public override void OnDropConnection(bool success, string extendedInfo) {
		LobbyGUI.Instance.FadeToWhite(DropConnection);
	}

	//called every time a player loads the game scene
	//use this to pass stuff from the lobbyplayer to the game player
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		Player game = gamePlayer.GetComponent<Player>();
		game.Username = lobby.Username;

		gamePlayer.name = game.Username;

		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
	}

	public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId) {
		GameObject playerObj;

		// get start position from base class
		// copied pretty much verbatim from the base networkmanager class
		// because the virtual version of this literally returns null. nothing but null.
		// and the thing that calls it handles that by saying "yup that's good run this right 'ere"
		// so here it is
		Transform startPos = GetStartPosition();
		if (startPos != null) {
			playerObj = (GameObject)Instantiate(gamePlayerPrefab, startPos.position, startPos.rotation);
		} else {
			playerObj = (GameObject)Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity);
		}

		Player player = playerObj.GetComponent<Player>();
		player.Initialize(++highestPlayerNum);
		return playerObj;
	}
}
