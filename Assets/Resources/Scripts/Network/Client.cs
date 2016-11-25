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

	public delegate void OnMessageReceived (NetworkMessage message);
	public event OnMessageReceived ThrowEvent = delegate { };
	
	private void Start() {
		networkManager = FindObjectOfType<NetworkManager>();

		Username = networkManager.Username;
		
		if (!networkManager.IsServer) {
			StartClient();
		}
	}

	public void StartClient() {
		NetworkClient = new NetworkClient();
		NetworkClient.RegisterHandler(ChatNetMessage.Code, OnChatNetMessage);
		NetworkClient.RegisterHandler(GridObjectPlacedNetMessage.Code, OnGridObjectPlacedNetMessage);
		NetworkClient.RegisterHandler(UpdatePlayerAssignment.Code, OnUpdatePlayerAssignmentMessage);
		NetworkClient.RegisterHandler(ClientJoinedMessage.Code, OnClientJoinedMessage);
		NetworkClient.Connect(networkManager.Ip, networkManager.Port);
		StartCoroutine(SendJoinMessage());
	}
	
	public IEnumerator SendJoinMessage() {
		yield return new WaitWhile(() => !NetworkClient.isConnected);
		NetworkClient.Send (ClientJoinedMessage.Code,new ClientJoinedMessage () {
			Username = Username
		});
	}

	private void OnClientJoinedMessage (NetworkMessage incoming) {
		ThrowEvent (incoming);
	}

	private void OnChatNetMessage (NetworkMessage incoming) {
		ThrowEvent (incoming);
	}
	
	private void OnGridObjectPlacedNetMessage(NetworkMessage incoming) {
		print ("object received");
		ThrowEvent (incoming);
	}
	
	private void OnUpdatePlayerAssignmentMessage(NetworkMessage incoming) {
		UpdatePlayerAssignment message = incoming.ReadMessage<UpdatePlayerAssignment>();
		PlayerId = message.PlayerId;
	}
}
