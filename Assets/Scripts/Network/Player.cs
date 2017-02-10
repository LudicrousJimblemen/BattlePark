/* Should only have command and rpc functions
 * everything else is done outside this class and passed into the iris
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Player : NetworkBehaviour {
	//"hotbar"
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
		
		//set hotbar
		//just like last time, TODO make it dynamic
		GridObjects[0] = GameManager.Instance.Objects[0];
	}
	
	[Command]
	public void CmdPlaceObject(int hotbarIndex, Vector3 position, int direction) {
		if (GridObjects[hotbarIndex] == null) 
			return;
		GameObject newObject = Instantiate(GridObjects[hotbarIndex].gameObject) as GameObject;
		newObject.GetComponent<GridObject> ().GridPosition = position;
		newObject.GetComponent<GridObject> ().Direction = (Direction) direction;
		NetworkServer.SpawnWithClientAuthority(newObject, gameObject);
	}
}
