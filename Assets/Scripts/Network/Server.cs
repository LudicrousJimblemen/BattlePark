using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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
	
	void Update() {
		if (!NetworkServer.active) {
			return;
		}
		
		string e = String.Empty;
		foreach (var connection in NetworkServer.connections) {
			if (connection != null) {
				e += connection.address + " ";
			}
		}
		print(e);
	}
	
	public void StartServer() {
		NetworkServer.RegisterHandler(GridObjectMessage.Code, OnGridObjectMessage);
		NetworkServer.Listen(networkManager.Port);
	}
	
	public void OnGridObjectMessage(NetworkMessage incoming) {
		GridObjectMessage message = incoming.ReadMessage<GridObjectMessage>();
		NetworkServer.SendToAll(GridObjectMessage.Code, message);
	}
}