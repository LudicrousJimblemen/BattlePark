using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {
	public GridObject[] GridObjects;
	
	[SyncVar]
	public string Username;

	public int PlayerNumber { get; set; }
	
	private void Start() {
		if (!isLocalPlayer) {
			return;
		}
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent(transform);
		print(PlayerNumber);
	}
	
	[Command]
	public void CmdPlaceObject(Vector3 position, Quaternion rotation) {
		GameObject newObject = Instantiate(GridObjects[0].gameObject, position, rotation) as GameObject;
		NetworkServer.SpawnWithClientAuthority(newObject, gameObject);
	}
}
