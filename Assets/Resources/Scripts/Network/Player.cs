/*
	To be used for client-side gameplay stuffs
	Should not function without client object
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour { 
	public bool Enabled = true;

	public bool CanSummon = true;

	private Grid grid;
	private GridOverlay gridOverlay;
	private ChatText chatText;
	private VerticalConstraint verticalConstraint;

	private GridPlaceholder gridPlaceholder;
	private GameObject summonedObject;

	private string[] hotbar = new string[9];

	private bool Paused;

	private Client Client;

	void Start () {
		DontDestroyOnLoad(this);
		grid = FindObjectOfType<Grid> ();
		gridOverlay = FindObjectOfType<GridOverlay> ();
		chatText = FindObjectsOfType<ChatText> ().First (x => x.name == "ChatText");
		verticalConstraint = FindObjectOfType<VerticalConstraint> ();

		Client = FindObjectOfType<Client> ();

		Client.ThrowGameplayEvent += ReceiveFromClient;

		//temporary, eventually hotbar slots will be defined dynamically
		hotbar[0] = "Sculpture";
	}

	private void Update () {
		Camera.main.GetComponent<CameraPan> ().enabled = !Paused;
		if (Paused) {
			Paused = !Input.GetKeyDown (KeyCode.Escape);
			return;
		}
		if (!Enabled) {
			return;
		}
		//returns which of the alpha keys were pressed this frame, preferring lower numbers
		for (int i = 0; i < 9; i++) {
			if (Input.GetKeyDown ((KeyCode) (49 + i))) {
				if (hotbar[i] != null) {
					if (CanSummon) {
						CanSummon = false;
					} else {
						Destroy (summonedObject);
					}
					summonedObject = SummonGridObject (hotbar[i]);
					gridPlaceholder = FindObjectOfType<GridPlaceholder> ();
					gridPlaceholder.Owner = Client.PlayerId;
					break;
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (summonedObject != null) {
				Destroy (summonedObject);
			} else {
				Paused = true;
			}
		}

		gridOverlay.ShowGrid = gridPlaceholder != null;
		if (gridPlaceholder == null) {
			return;
		} else {
			verticalConstraint.gameObject.SetActive (Input.GetKey (KeyCode.LeftControl));
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				EnableVerticalConstraint ();
			}

			if (Input.GetKeyDown (KeyCode.Z)) {
				gridPlaceholder.Rotate (-1);
			} else if (Input.GetKeyDown (KeyCode.X)) {
				gridPlaceholder.Rotate (1);
			}

			gridPlaceholder.Position (Input.mousePosition,Input.GetKey (KeyCode.LeftControl));
			gridPlaceholder.Snap ();

			if (Input.GetMouseButtonDown (0)) {
				gridPlaceholder.PlaceObject ();
			}
		}
	}

	public GameObject SummonGridObject (string name) {
		GameObject newGridObject = (GameObject) Instantiate (Resources.Load ("Prefabs/" + name));
		newGridObject.name = name;
		newGridObject.AddComponent<GridPlaceholder> ();
		return newGridObject;
	}

	public void EnableVerticalConstraint () {
		Vector3 correctedPosition = new Vector3 {
			x = Camera.main.transform.position.x,
			y = 0,
			z = Camera.main.transform.position.z
		};
		verticalConstraint.transform.position = gridPlaceholder.transform.position;
		verticalConstraint.transform.rotation = Quaternion.LookRotation (gridPlaceholder.transform.position - correctedPosition) * Quaternion.Euler (-90,0,0);
	}

	#region Messages
	public void ReceiveFromClient (NetworkMessage incoming) {
		switch (incoming.msgType) {
			case ClientJoinedMessage.Code:
				OnClientJoinedMessage (incoming);
				break;
			case ChatNetMessage.Code:
				OnChatNetMessage (incoming);
				break;
			case GridObjectPlacedNetMessage.Code:
				OnGridObjectPlacedNetMessage (incoming);
				break;
		}
	}
	private void OnClientJoinedMessage (NetworkMessage incoming) {
		ClientJoinedMessage message = incoming.ReadMessage<ClientJoinedMessage> ();
		chatText.AddText (String.Format (FindObjectOfType<LanguageManager> ().GetString ("chat.userJoined"),message.Username));
	}

	private void OnChatNetMessage (NetworkMessage incoming) {
		ChatNetMessage message = incoming.ReadMessage<ChatNetMessage> ();
		chatText.AddText (String.Format (FindObjectOfType<LanguageManager> ().GetString ("chat.chatMessage"),message.Username,message.Message)); 
	}

	private void OnGridObjectPlacedNetMessage (NetworkMessage incoming) {
		GridObjectPlacedNetMessage message = incoming.ReadMessage<GridObjectPlacedNetMessage> ();
		GameObject newGridObject = (GameObject) Instantiate (Resources.Load ("Prefabs/" + message.Type),message.Position,message.Rotation);

		GridObject component = newGridObject.GetComponent<GridObject> ();

		component.Deserialize (message.ObjectData);

		component.OnPlaced ();
		FindObjectOfType<Grid> ().Objects.Add (component.GridPosition (),component);
	}
	#endregion

	private void OnGUI () {
		GUI.Label (new Rect (0,0,100,100),"PlayerId: " + Client.PlayerId);
		GUI.Label (new Rect (0,15,500,100),"Username: " + Client.Username);
		FindObjectOfType<Mask> ().rectTransform.sizeDelta = new Vector2 (Screen.width / 3,Screen.height / 2);
		Scrollbar scrollbar = FindObjectOfType<Scrollbar> ();
		scrollbar.interactable = scrollbar.size != 1;
		if (Paused) {
			GUIStyle style = new GUIStyle ();
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 100;
			GUI.Label (new Rect (Screen.width / 2 - 50,Screen.height / 2 - 25,100,50),"Paused",style);
		}
	}
}