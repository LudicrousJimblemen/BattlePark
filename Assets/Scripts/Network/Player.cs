using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {
	
	[SyncVar]
	public string Username;
	
	void Start () {
		if (!isLocalPlayer) return;
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent (transform);
	}
	
	void Update () {
	
	}
}
