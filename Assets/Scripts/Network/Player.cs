/* Should only have command and rpc functions
 * everything else is done outside this class and passed into the iris
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {
	// "hotbar"
	public GridObject[] GridObjects;
	
	[SyncVar (hook = "UpdateUsername")]
	public string Username;
	private void UpdateUsername (string newUsername) {
		Username = newUsername;
		name = newUsername;
	}
	
	[SyncVar]
	public int PlayerNumber;
	
	private void Start() {
		if (!isLocalPlayer) {
			return;
		}
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent(transform);
		print(PlayerNumber);
		
		// set hotbar
		// just like last time, TODO make it dynamic
		GridObjects[0] = GameManager.Instance.Objects[0];
	}
	private void Update() {
		name = Username;
	}
	
	[Command]
	public void CmdPlaceObject(int hotbarIndex, Vector3 position, int direction) {
		if (GridObjects[hotbarIndex] == null) 
			return;
		GameObject newObject = Instantiate(GridObjects[hotbarIndex].gameObject,
		                                   Grid.Instance.SnapToGrid (position),
		                                   Quaternion.Euler(-90,0,direction * 90)
		                                   ) as GameObject;
		print (newObject.name);
		GridObject obj = newObject.GetComponent<GridObject> ();
		obj.GridPosition = position;
		obj.Direction = (Direction) direction;
		obj.OnPlaced ();
		NetworkServer.Spawn(newObject);
	}
}
