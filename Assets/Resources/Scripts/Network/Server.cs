using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : MonoBehaviour {
	public NetworkManager networkManager;
	
	void Start() {
		networkManager = FindObjectOfType<NetworkManager>();
		if (!networkManager.IsServer) {
			Destroy(this);
		} else {
			StartServer();
		}
	}
	
	public void StartServer() {
		NetworkServer.RegisterHandler(ChatNetMessage.Code, ResendMessage);
		NetworkServer.RegisterHandler(GridObjectPlacedNetMessage.Code, ResendMessage);
		NetworkServer.RegisterHandler(ClientJoinedMessage.Code, SendToClients);
		NetworkServer.Listen(networkManager.Port);
		FindObjectOfType<Client>().StartClient();
	}
	
	public void ResendMessage(NetworkMessage incoming) {
		switch (incoming.msgType) {
			case ChatNetMessage.Code:
				NetworkServer.SendToAll(incoming.msgType, incoming.ReadMessage<ChatNetMessage>());
				break;
			case GridObjectPlacedNetMessage.Code:
				NetworkServer.SendToAll(incoming.msgType, incoming.ReadMessage<GridObjectPlacedNetMessage>());
				break;
		}
	}
	
	public void SendToClients(NetworkMessage incoming) {
		//ClientJoinedMessage message = incoming.ReadMessage<ClientJoinedMessage>();
		NetworkServer.SendToClient(incoming.conn.connectionId, UpdatePlayerAssignment.Code, new UpdatePlayerAssignment() {
			PlayerID = incoming.conn.connectionId
		});
	}
}