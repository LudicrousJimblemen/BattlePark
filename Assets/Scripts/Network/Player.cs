using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {
	
	public GameObject w;
	
	[SyncVar]
	public string Username;
	
	void Start () {
		if (!isLocalPlayer) return;
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent (transform);
	}
	
	[Command]
	public void CmdPlaceObject (Vector3 Position, Quaternion Rotation) {
		w = Instantiate (w, Position, Rotation) as GameObject;
		NetworkServer.SpawnWithClientAuthority (w, gameObject);
	}
}
