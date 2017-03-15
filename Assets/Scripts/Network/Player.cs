/* Should only have command and rpc functions
 * everything else is done outside this class and passed into the iris
*/

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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

	[SyncVar(hook = "UpdateMoney")]
	public Money Money;
	private void UpdateMoney(Money newMoney) {
		Money = newMoney;
		GameGUI.Instance.Money.text = String.Format(LanguageManager.GetString("game.gui.numericCurrencySmall"), newMoney.Large, newMoney.Small);
	}

	[SyncVar]
	public int PlayerNumber;

	private void Start() {
		if (!isLocalPlayer)
			return;
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
		Camera.main.transform.SetParent(transform);

		// set hotbar
		// just like last time, TODO make it dynamic
		ObjectIndices = new int[9];
		for (int i = 0; i < 9; i++) {
			ObjectIndices[i] = GameManager.Instance.Objects[i] == null ? -1 : i;
		}
		for (int g = 0; g < 2; g++) {
			ServerSpawnPaths(g, Grid.Instance.GridSizeX, Grid.Instance.GridStepXZ, GameManager.Instance.ParkGates);
		}
		
		Money = new Money(700000000, 00);
	}

	public void PlaceObject(int hotbarIndex, Vector3? position, int direction) {
		if (position == null || ObjectIndices[hotbarIndex] == -1 || !getObject(hotbarIndex).Valid(position.Value, (Direction)direction, PlayerNumber))
			return;
		// boy it sure is a good thing that return is on a new line
		// really breaks up that one-liner into sizeable chunks
		CmdPlaceObject(ObjectIndices[hotbarIndex], position.Value, direction, PlayerNumber);
	}

	public GridObject getObject(int hotbarIndex) {
		return GameManager.Instance.Objects[ObjectIndices[hotbarIndex]];
	}

	[Command]
	public void CmdPlaceObject(int ObjIndex, Vector3 position, int direction, int playerNumber) {
		GameObject newObject = (GameObject)Instantiate(
			GameManager.Instance.Objects[ObjIndex].gameObject,
		    Grid.Instance.SnapToGrid(position, playerNumber),
		    Quaternion.identity);
		
		newObject.transform.Rotate(0, direction * 90, 0);
		newObject.transform.parent = GameManager.Instance.PlayerObjectParents[playerNumber - 1].transform;
		newObject.name = GameManager.Instance.Objects[ObjIndex].gameObject.name;
		
		GridObject obj = newObject.GetComponent<GridObject>();
		obj.GridPosition = position;
		obj.Direction = (Direction)direction;
		obj.Owner = playerNumber;
		
		NetworkServer.Spawn(newObject);
	}
	
	[Server]
	public void ServerSpawnPaths(int player, float sizeX, float step, Vector3[] parkGates) {
		print ("geg");
		for (int i = 0; i < (int)sizeX / 2; i++) {
			GridObject path = Instantiate(GameManager.Instance.Objects.First(x => x is GridPathAsphalt),
				                  parkGates[player] + (player * 2 - 1) * (0.5f + step / 2f + step * i) * Vector3.right,
				                  Quaternion.identity,
				                  GameManager.Instance.PlayerObjectParents[player].transform);
			path.Owner = player + 1;
		}
	}
	
	// TODO put this in person manager instead
	[Command]
	public void CmdSpawnPerson(Vector3 position, int owner) {
		GameObject person = Instantiate(GameManager.Instance.PersonObj, position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(person);
		person.GetComponent<Pathfinding.PathWalker> ().graph = GameManager.Instance.Graphs[owner-1];
	}
}
