using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour {
	private NetworkManager networkManager;
	
	public NetworkClient NetworkClient;

	private GameObject SummonedObject;
	
	private string[] Hotbar = new string[9];
	private bool canSummon = true;
	
	private GridPlaceholder gridPlaceholder;
	
	int[] PlayerList;

	void Start() {
		networkManager = FindObjectOfType<NetworkManager>();
		
		//temporary, eventually hotbar slots will be defined dynamically
		Hotbar[0] = "Sculpture";
		Hotbar[1] = "Tree";
		if (!networkManager.IsServer) StartClient();
	}
	
	void Update() {
		//print (canSummon);
		//returns which of the alpha keys were pressed this frame, preferring lower numbers
		for (int i = 0; i < 9; i ++) {
			//49 50 51 52 53 54 55 56 57
			if (Input.GetKeyDown((KeyCode)(49 + i))) {
				if (Hotbar[i] != null) {
					if (canSummon) {
						SummonedObject = SummonGridObject (Hotbar[i]);
						canSummon = false;
					} else {
						Destroy (SummonedObject);
						SummonedObject = SummonGridObject (Hotbar[i]);
					}
					gridPlaceholder = FindObjectOfType<GridPlaceholder>();
					break;
				}
			}
		}
		if (gridPlaceholder == null) return;
		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			gridPlaceholder.EnableVerticalConstraint ();
		}
		
		if (Input.GetKeyDown (KeyCode.Z)) {
			gridPlaceholder.Rotate (-1);
		} else if (Input.GetKeyDown (KeyCode.X)) {
			gridPlaceholder.Rotate (1);
		}
		gridPlaceholder.Raycast (Input.GetKey (KeyCode.LeftControl));
	}
	
	public void AllowSummons () {
		canSummon = true;
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
		NetworkClient.RegisterHandler (UpdatePlayerListMessage.Code, OnUpdatePlayerListMessage);
		NetworkClient.RegisterHandler (ClientJoinedMessage.Code, OnClientJoinedMessage);
		NetworkClient.Connect(networkManager.Ip, networkManager.Port);
		StartCoroutine (sendJoinMessage ());
	}
	public IEnumerator sendJoinMessage () {
		
		yield return new WaitWhile(() => !NetworkClient.isConnected);
		NetworkClient.Send (ClientJoinedMessage.Code, new ClientJoinedMessage () {
			ConnectionId = NetworkClient.connection.connectionId
		});
	}
	
	void OnClientJoinedMessage (NetworkMessage incoming) {
		return;
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
	
	public void OnUpdatePlayerListMessage (NetworkMessage incoming) {
		UpdatePlayerListMessage message = incoming.ReadMessage<UpdatePlayerListMessage>();
		PlayerList = message.PlayerList;
	}
}
