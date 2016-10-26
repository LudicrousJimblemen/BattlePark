using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour {
	public NetworkManager networkManager;
	public NetworkClient networkClient;

	void Start() {
		networkManager = FindObjectOfType<NetworkManager>();
		
		StartClient();
	}
	
	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			Instantiate(FindObjectOfType<Grid>().PlaceholderObjectPrefab, Vector3.zero, Quaternion.identity);
		}
	}

	public void StartClient() {
		networkClient = new NetworkClient();
		networkClient.RegisterHandler(GridObjectMessage.Code, OnGridObjectMessage);
		networkClient.Connect(networkManager.Ip, networkManager.Port);
	}
	
	public void SendGridObjectMessage(GameObject placeholder) {
		networkClient.Send(GridObjectMessage.Code, new GridObjectMessage { position = placeholder.transform.position });
	}
	
	public void OnGridObjectMessage(NetworkMessage incoming) {
		GridObjectMessage message = incoming.ReadMessage<GridObjectMessage>();
		Instantiate(FindObjectOfType<Grid>().GridObjectPrefab, message.position, Quaternion.identity);
	}
}
