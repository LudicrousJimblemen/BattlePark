using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour {
	public NetworkClient NetworkClient;
	public bool CanSummon = true;
	
	private NetworkManager networkManager;

	private GameObject summonedObject;
	
	private string[] hotbar = new string[9];
	
	private GridPlaceholder gridPlaceholder;
	private VerticalConstraint verticalConstraint;
	
	int[] PlayerList;

	void Start() {
		networkManager = FindObjectOfType<NetworkManager>();
		
		//temporary, eventually hotbar slots will be defined dynamically
		hotbar[0] = "Sculpture";
		hotbar[1] = "Tree";
		hotbar[2] = "Path";
		
		verticalConstraint = FindObjectOfType<VerticalConstraint>();
		
		if (!networkManager.IsServer) {
			StartClient();
		}
	}
	
	void Update() {
		//returns which of the alpha keys were pressed this frame, preferring lower numbers
		for (int i = 0; i < 9; i++) {
			if (Input.GetKeyDown((KeyCode)(49 + i))) {
				if (hotbar[i] != null) {
					if (CanSummon) {
						CanSummon = false;
					} else {
						Destroy(summonedObject);
					}
					summonedObject = SummonGridObject(hotbar[i]);
					gridPlaceholder = FindObjectOfType<GridPlaceholder>();
					gridPlaceholder.Owner = NetworkClient.connection.connectionId;
					break;
				}
			}
		}
		
		verticalConstraint.gameObject.SetActive(Input.GetKey(KeyCode.LeftControl));
		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			EnableVerticalConstraint();
		}
		
		if (gridPlaceholder == null) {
			return;
		}
		
		if (Input.GetKeyDown(KeyCode.Z)) {
			gridPlaceholder.Rotate(1);
		} else if (Input.GetKeyDown(KeyCode.X)) {
			gridPlaceholder.Rotate(-1);
		}
		
		gridPlaceholder.Position(Input.GetKey(KeyCode.LeftControl));
		gridPlaceholder.Snap();
		
		if (Input.GetMouseButtonDown(0)) {
			gridPlaceholder.PlaceObject();
		}
	}
	
	public void EnableVerticalConstraint() {
		Vector3 correctedPosition = new Vector3(
			Camera.main.transform.position.x,
			0,
			Camera.main.transform.position.z
		);
		verticalConstraint.transform.position = gridPlaceholder.transform.position;
		verticalConstraint.transform.rotation = Quaternion.LookRotation(gridPlaceholder.transform.position - correctedPosition) * Quaternion.Euler(-90, 0, 0);
	}
	
	public GameObject SummonGridObject(string name) {
		GameObject newGridObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + name));
		
		newGridObject.AddComponent<GridPlaceholder>();
		
		return newGridObject;
	}

	public void StartClient() {
		NetworkClient = new NetworkClient();
		NetworkClient.RegisterHandler(ChatNetMessage.Code, OnChatNetMessage);
		NetworkClient.RegisterHandler(GridObjectPlacedNetMessage.Code, OnGridObjectPlacedNetMessage);
		NetworkClient.RegisterHandler(UpdatePlayerListMessage.Code, OnUpdatePlayerListMessage);
		NetworkClient.RegisterHandler(ClientJoinedMessage.Code, OnClientJoinedMessage);
		NetworkClient.Connect(networkManager.Ip, networkManager.Port);
		StartCoroutine(SendJoinMessage());
	}
	
	public IEnumerator SendJoinMessage() {
		yield return new WaitWhile(() => !NetworkClient.isConnected);
		NetworkClient.Send(ClientJoinedMessage.Code, new ClientJoinedMessage() {
			ConnectionId = NetworkClient.connection.connectionId
		});
		print(NetworkClient.connection.connectionId);
	}
	
	private void OnClientJoinedMessage(NetworkMessage incoming) {
		return;
	}
	
	private void OnChatNetMessage(NetworkMessage incoming) {
		ChatNetMessage message = incoming.ReadMessage<ChatNetMessage>();
		print(message.Message);
	}
	
	private void OnGridObjectPlacedNetMessage(NetworkMessage incoming) {
		GridObjectPlacedNetMessage message = incoming.ReadMessage<GridObjectPlacedNetMessage>();
		GameObject newGridObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + message.Type));
		
		GridObject component = newGridObject.GetComponent<GridObject>();
		
		component.Deserialize(message.ObjectData);
		newGridObject.transform.position = new Vector3(
			component.X * FindObjectOfType<Grid>().GridXZ,
			component.Y * FindObjectOfType<Grid>().GridY,
			component.Z * FindObjectOfType<Grid>().GridXZ
		);
		newGridObject.transform.rotation = Quaternion.Euler(-90, 0, (int)component.Direction * 90);
		
		component.OnPlaced();
	}
	
	private void OnUpdatePlayerListMessage(NetworkMessage incoming) {
		UpdatePlayerListMessage message = incoming.ReadMessage<UpdatePlayerListMessage>();
		PlayerList = message.PlayerList;
	}
}
