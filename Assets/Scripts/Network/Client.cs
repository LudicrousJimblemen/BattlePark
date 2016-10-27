using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Client : MonoBehaviour {
	private Grid grid;
	
	public NetworkManager networkManager;
	public NetworkClient networkClient;

	void Start() {
		grid = FindObjectOfType<Grid>();
		networkManager = FindObjectOfType<NetworkManager>();
		StartClient();
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			GameObject newSculpture = (GameObject)Instantiate(Objects.Sculpture);
			newSculpture.AddComponent<GridPlaceholder>();
		}
	}

	public void StartClient() {
		networkClient = new NetworkClient();
		NetworkServer.RegisterHandler(ChatNetMessage.Code, OnChatNetMessage);
		networkClient.Connect(networkManager.Ip, networkManager.Port);
	}
	
	public void OnChatNetMessage(NetworkMessage incoming) {
		 ChatNetMessage message = incoming.ReadMessage<ChatNetMessage>();
		 print(message.Message);
	}
}
