/*
	To be used for client-side networking stuffs
	Can exist independent of player
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : MonoBehaviour {

	public NetworkClient NetworkClient;

	public int PlayerId;
	public string Username;
	private NetworkManager networkManager;

	public delegate void OnGameplayMessageReceived(NetworkMessage message);
	public event OnGameplayMessageReceived ThrowGameplayEvent = delegate { };

	public delegate void OnConnected();
	public event OnConnected ThrowConnectionEvent = delegate { };

	private void Start() {
		networkManager = FindObjectOfType<NetworkManager>();

		Username = networkManager.Username;

		if(!networkManager.IsServer) {
			StartClient();
		}
	}

	public void StartClient() {
		NetworkClient = new NetworkClient();
		NetworkClient.RegisterHandler(ChatNetMessage.Code,OnGameplayMessage);
		NetworkClient.RegisterHandler(GridObjectPlacedNetMessage.Code,OnGameplayMessage);
		NetworkClient.RegisterHandler(ClientJoinedMessage.Code,OnGameplayMessage);
		NetworkClient.RegisterHandler(UpdatePlayerAssignment.Code,OnUpdatePlayerAssignmentMessage);
		NetworkClient.Connect(networkManager.Ip,networkManager.Port);
		StartCoroutine(SendJoinMessage());
	}

	public IEnumerator SendJoinMessage() {
		yield return new WaitWhile(() => !NetworkClient.isConnected);
		NetworkClient.Send(ClientJoinedMessage.Code,new ClientJoinedMessage() {
			Username = Username
		});
	}

	private void OnGameplayMessage(NetworkMessage incoming) {
		ThrowGameplayEvent(incoming);
	}

	private void OnUpdatePlayerAssignmentMessage(NetworkMessage incoming) {
		UpdatePlayerAssignment message = incoming.ReadMessage<UpdatePlayerAssignment>();
		PlayerId = message.PlayerId;
		ThrowConnectionEvent();
		print("Connected");
	}
}
