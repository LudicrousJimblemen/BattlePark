using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

public class Client : NetworkLobbyManager {
	public static Client Instance;
	
	[HideInInspector]
	public bool IsHost;
	
	public LobbyPlayer localPlayer;
	
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
	
	IEnumerator WaitForNetworkManager () {
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
	public override void OnDropConnection (bool success, string extendedInfo) {
		LobbyGUI.Instance.FadeToWhite (DropConnection);
	}
	void DropConnection () {
		StopMatchMaker();
		if (IsHost) StopHost (); else StopClient ();
		if (localPlayer != null) {
			localPlayer.RemovePlayer ();
			Destroy (localPlayer.gameObject);
		}
		LobbyGUI.Instance.LoadMain ();
	}
	public override void OnLobbyServerPlayersReady () {
		print ("mate, we're shipshape");
		StartCoroutine(StartGame());
	}
	
	IEnumerator StartGame () {
		for (int i = 0; i < lobbySlots.Length; i++) {
			if (lobbySlots[i] == null) break;
			((LobbyPlayer)lobbySlots[i]).RpcPrepareReady();
		}
		yield return new WaitForSeconds (2.0f);
		ServerChangeScene(playScene);
	}
	
	//called every time a player loads the game scene
	//use this to pass stuff from the lobbyplayer to the game player
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer> ();
		Player game = gamePlayer.GetComponent<Player> ();
		game.Username = lobby.Username;
		
		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
	}
}
