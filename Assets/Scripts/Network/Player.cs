/* Should only have command and rpc functions
 * everything else is done outside this class and passed into the iris
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {
	//"hotbar"
	//list of indices relating to objects in the gamemanager's list of objects
	public int[] ObjectIndices;
	
	[SyncVar (hook = "UpdateUsername")]
	public string Username;
	private void UpdateUsername (string newUsername) {
		Username = newUsername;
		name = newUsername;
	}
	
	[SyncVar]
	public int PlayerNumber;
	
	private void Start() {
		if (!isLocalPlayer) 
			return;
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent(transform);
		print("PlayerNumber: " + PlayerNumber);
		
		//set hotbar
		//just like last time, TODO make it dynamic
		ObjectIndices = new int[9];
		ObjectIndices[0] = 0;
		
		//for everything not defined; increment initial value upon adding a new hotbar thing
		//temporarily, of course
		//totally won't make it into the final game
		for (int i = 1; i < ObjectIndices.Length; i ++) {
			ObjectIndices[i] = -1;
		}
	}
	private void Update() {
		name = Username;
	}
	
	public void PlaceObject (int hotbarIndex, Vector3? position, int direction) {
		if (position == null || ObjectIndices[hotbarIndex] == -1)
			return;
		print (hotbarIndex);
		
		CmdPlaceObject (ObjectIndices[hotbarIndex], position.Value, direction);
	}
	
	[Command]
	public void CmdPlaceObject(int ObjIndex, Vector3 position, int direction) {
		GameObject newObject = Instantiate(GameManager.Instance.Objects[ObjIndex].gameObject,
		                                   Grid.Instance.SnapToGrid (position),
		                                   Quaternion.Euler(-90,0,direction * 90)
		                                   ) as GameObject;
		GridObject obj = newObject.GetComponent<GridObject> ();
		obj.GridPosition = position;
		obj.Direction = (Direction) direction;
		NetworkServer.Spawn(newObject);
	}
}
