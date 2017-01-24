/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

public class Client : MonoBehaviour {
	public static Client Instance;
	
	[HideInInspector]
	public bool IsHost;
	[HideInInspector]
	public NetworkID currentNetID;
	[HideInInspector]
	public bool Ready;
	
	private void Awake() {
		if (FindObjectsOfType<Client>().Length > 1) {
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad(gameObject);
		Instance = this;
		//print (NetworkManager.singleton == null);
		//StartCoroutine(WaitForNetworkManager());
	}
	
	public void StartMatchMaker() {
		NetworkManager.singleton.StartMatchMaker();
	}
	
	IEnumerator WaitForNetworkManager () {
		while (NetworkManager.singleton == null) {
			yield return new WaitForEndOfFrame();
		}
		//NetworkManager.singleton.StartMatchMaker();
	}
	
	public void CreateMatch(string roomName, uint size, NetworkMatch.DataResponseDelegate<MatchInfo> callback) {
		NetworkManager.singleton.matchMaker.CreateMatch(
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
					NetworkManager.singleton.StartHost(matchInfo);
					currentNetID = matchInfo.networkId;
				}
				callback(success, extendedInfo, matchInfo);
			});
	}
	
	public void ListMatches(int maxResults, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> callback) {
		NetworkManager.singleton.matchMaker.ListMatches(
			0,
			maxResults,
			String.Empty,
			true,
			0,
			0,
			callback);
	}
	
	public void JoinMatch(NetworkID id, NetworkMatch.DataResponseDelegate<MatchInfo> callback) {
		NetworkManager.singleton.matchMaker.JoinMatch(
			id,
			String.Empty,
			String.Empty,
			String.Empty,
			0,
			0,
			(success, extendedInfo, matchInfo) => {
				IsHost = success;
				if (success) {
					NetworkManager.singleton.StartClient(matchInfo);
					currentNetID = matchInfo.networkId;
				}
				callback(success, extendedInfo, matchInfo);
			});
	}
	public void OnDestroyMatch (bool success, string extendedInfo) {
		NetworkManager.singleton.StopMatchMaker();
		if (IsHost) NetworkManager.singleton.StopHost();
		else NetworkManager.singleton.StopClient();
	}
	public void StartGame () {
		if (!IsHost) return;
	}
}
*/