using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : MonoBehaviour
{
	public NetworkManager networkManager;
	
	int[] PlayerList;
	private int currentPlayerCount = 0;
	
	void Start()
	{
		networkManager = FindObjectOfType<NetworkManager>();
		if (!networkManager.IsServer) {
			Destroy(this);
		} else {
			StartServer();
		}
	}
	
	public void StartServer()
	{
		NetworkServer.RegisterHandler(ChatNetMessage.Code, ResendMessage);
		NetworkServer.RegisterHandler(GridObjectPlacedNetMessage.Code, ResendMessage);
		NetworkServer.RegisterHandler(ClientJoinedMessage.Code, SendToClients);
		NetworkServer.Listen(networkManager.Port);
		FindObjectOfType<Client>().StartClient();
		print ("e");
	}
	
	public void ResendMessage(NetworkMessage incoming)
	{
		switch (incoming.msgType) {
			case ChatNetMessage.Code:
				NetworkServer.SendToAll(incoming.msgType, incoming.ReadMessage<ChatNetMessage>());
				break;
			case GridObjectPlacedNetMessage.Code:
				print("<Server> Received object");
				NetworkServer.SendToAll(incoming.msgType, incoming.ReadMessage<GridObjectPlacedNetMessage>());
				break;
		}
	}
	
	public void SendToClients(NetworkMessage incoming)
	{
		ClientJoinedMessage message = incoming.ReadMessage<ClientJoinedMessage> ();
		int playerIDToAssign = ++currentPlayerCount;
		NetworkServer.SendToClient (message.ConnectionId,UpdatePlayerAssignment.Code,new UpdatePlayerAssignment () {
			PlayerID = playerIDToAssign
		});
		/*
		int id = incoming.ReadMessage<ClientJoinedMessage>().ConnectionId;
		int freeIndex = -1;
		for (int i = 0; i < PlayerList.Length; i++) {
			if (PlayerList[i] == -1) {
				freeIndex = i;
				break;
			}
		}
		if (freeIndex == -1) {
			NetworkServer.SendToAll(incoming.msgType, new UpdatePlayerListMessage() {
				PlayerList = null
			});
		} else {
			PlayerList[freeIndex] = id;
			NetworkServer.SendToAll(incoming.msgType, new UpdatePlayerListMessage() {
				PlayerList = PlayerList
			});
		}
		*/
	}
}