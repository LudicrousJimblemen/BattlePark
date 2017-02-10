using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {
	public GridObject[] GridObjects;
	
	[SyncVar]
	public string Username;
	
	public int selectedObjectIndex;
	public int selectedObjectDirection;

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
	public void CmdPlaceObject(Vector3 position) {
		if (GameManager.Instance.Objects[selectedObjectIndex] == null) 
			return;
		GameObject newObject = Instantiate(GameManager.Instance.Objects[selectedObjectIndex].gameObject) as GameObject;
		newObject.GetComponent<GridObject> ().GridPosition = position;
		newObject.GetComponent<GridObject> ().Direction = (Direction) selectedObjectDirection;
		NetworkServer.SpawnWithClientAuthority(newObject, gameObject);
	}
}
