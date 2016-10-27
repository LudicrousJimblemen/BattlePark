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
		NetworkServer.RegisterHandler(ChatNetMessage.Code, OnChatNetMessage);
		NetworkServer.Listen(networkManager.Port);
	}
	
	public void OnChatNetMessage(NetworkMessage incoming) {
		 ChatNetMessage message = incoming.ReadMessage<ChatNetMessage>();
		 print(message.Message);
	}
}