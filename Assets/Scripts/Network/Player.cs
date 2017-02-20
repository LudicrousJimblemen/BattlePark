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

	[SyncVar(hook = "UpdateUsername")]
	public string Username;
	private void UpdateUsername(string newUsername) {
		Username = newUsername;
		name = newUsername;
	}

	[SyncVar]
	public int PlayerNumber;

	private void Start() {
		if(!isLocalPlayer)
			return;
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent(transform);
		print("PlayerNumber: " + PlayerNumber);

		// set hotbar
		// just like last time, TODO make it dynamic
		ObjectIndices = new int[9];
		for(int i = 0; i < 9; i++) {
			ObjectIndices[i] = GameManager.Instance.Objects[i] == null ? -1 : i;
		}
	}
	private void Update() {
		name = Username;
	}

	public void PlaceObject(int hotbarIndex,Vector3? position,int direction) {
		if(position == null || ObjectIndices[hotbarIndex] == -1 || !getObject(hotbarIndex).Valid (position.Value, (Direction)direction, PlayerNumber))
			return;
		// boy it sure is a good thing that return is on a new line
		// really breaks up that one-liner into sizeable chunks
		print (hotbarIndex);
        CmdPlaceObject (ObjectIndices[hotbarIndex], position.Value, direction, PlayerNumber);
	}

	public GridObject getObject (int hotbarIndex) {
		return GameManager.Instance.Objects[ObjectIndices[hotbarIndex]];
    }
	
	[Command]
	public void CmdPlaceObject(int ObjIndex, Vector3 position, int direction, int playerNumber) {
		GameObject newObject = Instantiate(GameManager.Instance.Objects[ObjIndex].gameObject,
										   Grid.Instance.SnapToGrid(position, playerNumber),
										   Quaternion.Euler(-90,0,direction * 90),
										   GameManager.Instance.PlayerObjectParents[playerNumber - 1].transform
										   ) as GameObject;
		newObject.name = GameManager.Instance.Objects[ObjIndex].gameObject.name;
		GridObject obj = newObject.GetComponent<GridObject> ();
		obj.GridPosition = position;
		obj.Direction = (Direction) direction;
		obj.Owner = playerNumber;
		NetworkServer.Spawn(newObject);
	}

	[Command]
	public void CmdSpawnPerson (Vector3 position) {
		GameObject person = Instantiate(GameManager.Instance.PersonObj,position,Quaternion.identity) as GameObject;
		NetworkServer.Spawn(person);
		person.GetComponent<AIPath>().target = FindObjectOfType<AstarPath>().transform;
	}
}
