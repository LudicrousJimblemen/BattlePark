using System;
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
		NetworkServer.Listen(networkManager.Port);
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
}