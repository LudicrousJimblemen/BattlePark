using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : MonoBehaviour {
	public bool Enabled = true;
	
	public NetworkClient NetworkClient;
	public bool CanSummon = true;
	
	public int PlayerId;
	public string Username;
	
	private Grid grid;
	private NetworkManager networkManager;
	private GridOverlay gridOverlay;
	private ChatText chatText;
	private VerticalConstraint verticalConstraint;
	
	private GridPlaceholder gridPlaceholder;
	private GameObject summonedObject;
	
	private string[] hotbar = new string[9];
	
	private bool Paused;
	
	private void Start() {
		grid = FindObjectOfType<Grid>();
		networkManager = FindObjectOfType<NetworkManager>();
		gridOverlay = FindObjectOfType<GridOverlay>();
		chatText = FindObjectsOfType<ChatText>().First(x => x.name == "ChatText");
		verticalConstraint = FindObjectOfType<VerticalConstraint>();

		Username = networkManager.Username;
		//temporary, eventually hotbar slots will be defined dynamically
		hotbar[0] = "Sculpture";
		
		if (!networkManager.IsServer) {
			StartClient();
		}
	}
	
	private void Update() {
		Camera.main.GetComponent<CameraPan>().enabled = !Paused;
		if (Paused) {
			Paused = !Input.GetKeyDown(KeyCode.Escape);
			return;
		}
		if (!Enabled) {
			return;
		}
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
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (summonedObject != null) {
				Destroy(summonedObject);
			} else {
				Paused = true;
			}
		}
		
		gridOverlay.ShowGrid = gridPlaceholder != null;
		if (gridPlaceholder == null) {
			return;
		} else {
			verticalConstraint.gameObject.SetActive(Input.GetKey(KeyCode.LeftControl));
			if (Input.GetKeyDown(KeyCode.LeftControl)) {
				EnableVerticalConstraint();
			}
		
			if (Input.GetKeyDown(KeyCode.Z)) {
				gridPlaceholder.Rotate(-1);
			} else if (Input.GetKeyDown(KeyCode.X)) {
				gridPlaceholder.Rotate(1);
			}
			
			gridPlaceholder.Position(Input.mousePosition, Input.GetKey(KeyCode.LeftControl));
			gridPlaceholder.Snap();
			
			if (Input.GetMouseButtonDown(0)) {
				gridPlaceholder.PlaceObject();
			}
		}
	}
	
	private void OnGUI() {
		GUI.Label(new Rect(0, 0, 100, 100), "PlayerId: " + PlayerId);
		GUI.Label (new Rect (0,15,500,100),"Username: " + Username);
		FindObjectOfType<Mask> ().rectTransform.sizeDelta = new Vector2 (Screen.width/3,Screen.height / 2);
		if (Paused) {
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 100;
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Paused", style);
		}
	}
	
	public void EnableVerticalConstraint() {
		Vector3 correctedPosition = new Vector3 {
			x = Camera.main.transform.position.x,
			y = 0,
			z = Camera.main.transform.position.z
		};
		verticalConstraint.transform.position = gridPlaceholder.transform.position;
		verticalConstraint.transform.rotation = Quaternion.LookRotation(gridPlaceholder.transform.position - correctedPosition) * Quaternion.Euler(-90, 0, 0);
	}
	
	public GameObject SummonGridObject(string name) {
		GameObject newGridObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + name));
		newGridObject.name = name;
		newGridObject.AddComponent<GridPlaceholder>();
		return newGridObject;
	}

	public void StartClient() {
		NetworkClient = new NetworkClient();
		NetworkClient.RegisterHandler(ChatNetMessage.Code, OnChatNetMessage);
		NetworkClient.RegisterHandler(GridObjectPlacedNetMessage.Code, OnGridObjectPlacedNetMessage);
		NetworkClient.RegisterHandler(UpdatePlayerAssignment.Code, OnUpdatePlayerAssignmentMessage);
		NetworkClient.RegisterHandler(ClientJoinedMessage.Code, OnClientJoinedMessage);
		NetworkClient.Connect(networkManager.Ip, networkManager.Port);
		StartCoroutine(SendJoinMessage());
	}
	
	public IEnumerator SendJoinMessage() {
		yield return new WaitWhile(() => !NetworkClient.isConnected);
		NetworkClient.Send (ClientJoinedMessage.Code,new ClientJoinedMessage () {
			Username = Username
		});
	}
	
	private void OnClientJoinedMessage(NetworkMessage incoming) {
		ClientJoinedMessage message = incoming.ReadMessage<ClientJoinedMessage>();
		chatText.AddText(String.Format(FindObjectOfType<LanguageManager>().GetString("chat.userJoined"), message.Username));
	}
	
	private void OnChatNetMessage(NetworkMessage incoming) {
		ChatNetMessage message = incoming.ReadMessage<ChatNetMessage>();
		chatText.AddText(String.Format(FindObjectOfType<LanguageManager>().GetString("chat.chatMessage"), message.Username, message.Message));
	}
	
	private void OnGridObjectPlacedNetMessage(NetworkMessage incoming) {
		GridObjectPlacedNetMessage message = incoming.ReadMessage<GridObjectPlacedNetMessage>();
		GameObject newGridObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + message.Type), message.Position, message.Rotation);
		
		GridObject component = newGridObject.GetComponent<GridObject>();

		component.Deserialize(message.ObjectData);
		
		component.OnPlaced();
		FindObjectOfType<Grid>().Objects.Add(component.GridPosition(), component);
	}
	
	private void OnUpdatePlayerAssignmentMessage(NetworkMessage incoming) {
		UpdatePlayerAssignment message = incoming.ReadMessage<UpdatePlayerAssignment>();
		PlayerId = message.PlayerId;
	}
}
