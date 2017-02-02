using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : NetworkBehaviour {

	public GridObject[] GridObjects;
	
	[SyncVar]
	public string Username;

	[HideInInspector]
	int PlayerNumber;
	
	void Start () {
		if (!isLocalPlayer) return;
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent (transform);
		print(PlayerNumber);
	}
	
	public void Initialize (int playerNum) {
		PlayerNumber = playerNum;
	}
	
	[Command]
	public void CmdPlaceObject (Vector3 Position, Quaternion Rotation) {
		GameObject newObj = Instantiate (GridObjects[0].gameObject, Position, Rotation) as GameObject;
		NetworkServer.SpawnWithClientAuthority (newObj, gameObject);
	}
}
