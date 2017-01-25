using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {
	
	[SyncVar]
	public string Username;
	
	void Start () {
		if (!isLocalPlayer) return;
		Camera.main.transform.SetParent (transform);
	}
	
	void Update () {
	
	}
}
