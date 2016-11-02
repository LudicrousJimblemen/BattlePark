using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour {
	private NetworkManager networkManager;
	
	public NetworkClient NetworkClient;

	private GameObject SummonedObject;
	
	private string[] Hotbar = new string[9];
	private bool canSummon = true;

	void Start() {
		networkManager = FindObjectOfType<NetworkManager>();
		StartClient();
		
		//temporary, eventually hotbar slots will be defined dynamically
		Hotbar[0] = "Sculpture";
		Hotbar[1] = "Tree";
	}
	
	void Update() {
		//returns which of the alpha keys were pressed this frame, preferring lower numbers
		for (int i = 0; i < 9; i ++) {
			//49 50 51 52 53 54 55 56 57
			if (Input.GetKeyDown((KeyCode)(49 + i))) {
				if (Hotbar[i] != null && canSummon) {
					SummonedObject = SummonGridObject (Hotbar[i]);
					canSummon = false;
				}
				break;
			}
		}
		/*
		if (Input.GetKeyDown(KeyCode.Q)) {
			SummonGridObject("Sculpture");
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			SummonGridObject("Tree");
		}
		*/
	}
	
	public GameObject SummonGridObject(string name) {
		GameObject newGridObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + name));
		
		GridPlaceholder component = newGridObject.AddComponent<GridPlaceholder>();
		component.Type = name;
		return newGridObject;
	}

	public void StartClient() {
		NetworkClient = new NetworkClient();
		NetworkClient.RegisterHandler(ChatNetMessage.Code, OnChatNetMessage);
		NetworkClient.RegisterHandler(GridObjectPlacedNetMessage.Code, OnGridObjectPlacedNetMessage);
		NetworkClient.Connect(networkManager.Ip, networkManager.Port);
	}
	
	public void OnChatNetMessage(NetworkMessage incoming) {
		 ChatNetMessage message = incoming.ReadMessage<ChatNetMessage>();
		 print(message.Message);
	}
	
	public void OnGridObjectPlacedNetMessage(NetworkMessage incoming) {
		GridObjectPlacedNetMessage message = incoming.ReadMessage<GridObjectPlacedNetMessage>();
		
		GameObject newGridObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + message.Type));
		GridObject component = newGridObject.GetComponent<GridObject>();
		
		newGridObject.transform.position = message.Position;
		component.Deserialize(message.ObjectData);
	}
}
