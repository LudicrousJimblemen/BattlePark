using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : MonoBehaviour {
	public NetworkManager networkManager;
	int PlayerNumberCounter = 1;
	
	List<int> Players = new List<int> ();
	
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
		NetworkServer.Listen(networkManager.Ip, networkManager.Port);
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
	
	public void SendToClients (NetworkMessage incoming) {
		int id = incoming.ReadMessage<ClientJoinedMessage>().ConnectionId;
		Players.Add (id);
		NetworkServer.SendToAll (incoming.msgType, new UpdatePlayerListMessage () {
			PlayerList = Players
		});
	}
}