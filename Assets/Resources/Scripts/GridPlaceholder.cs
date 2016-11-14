using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GridPlaceholder : MonoBehaviour {
	private NetworkClient client;
	private new Camera camera;
	private Grid grid;
	private VerticalConstraint verticalConstraint;
	
	public GridObject GridObject;
	
	public int Owner;
	
	void Start() {
		client = FindObjectOfType<Client>().NetworkClient;
		camera = FindObjectOfType<Camera>();
		grid = FindObjectsOfType<Grid>().First(x => x.playerId == Owner);
		print(grid == null);
		GridObject = GetComponent<GridObject>();
	}
	
	public void Griddify() {
		transform.rotation = Quaternion.Euler(-90, 0, (int)GridObject.Direction * 90);
		
		transform.position = new Vector3 { //snap to grid
			x = Mathf.Round(transform.position.x / grid.GridXZ) * grid.GridXZ,
			z = Mathf.Round(transform.position.z / grid.GridXZ) * grid.GridXZ,
			y = Mathf.Round(Mathf.Clamp(transform.position.y / grid.GridY, 0, Mathf.Infinity)) * grid.GridY
		};
	}
	
	public void EnableVerticalConstraint() {
		Vector3 correctedPosition = new Vector3(
			                            camera.transform.position.x,
			                            0,
			                            camera.transform.position.z
		                            );
		verticalConstraint.transform.position = transform.position;
		verticalConstraint.transform.rotation = Quaternion.LookRotation(transform.position - correctedPosition) * Quaternion.Euler(-90, 0, 0);
	}
	
	public void PlaceObject() {
		GridObject.X = Mathf.RoundToInt(transform.position.x / grid.GridXZ);
		GridObject.Y = Mathf.RoundToInt(transform.position.y / grid.GridY);
		GridObject.Z = Mathf.RoundToInt(transform.position.z / grid.GridXZ);
		client.Send(GridObjectPlacedNetMessage.Code, new GridObjectPlacedNetMessage() {
			ConnectionId = client.connection.connectionId,
			//N A M E ( C L O N E )
			//0 1 2 3 4 5 6 7 8 9 10
			Type = name.Substring(0, name.Length - 7),
			ObjectData = GridObject.Serialize()
		});
		FindObjectOfType<Client>().AllowSummons();
		Destroy(gameObject);
	}
	
	public void Rotate(int direction) {
		GridObject.Direction += direction;
		
		if (GridObject.Direction > (Direction)3) {
			GridObject.Direction = (Direction)0;
		}
		if (GridObject.Direction < (Direction)0) {
			GridObject.Direction = (Direction)3;
		}
	}
	
	public void Raycast(bool UseVerticalConstraint = false) {
		RaycastHit hit;
		if (UseVerticalConstraint) {
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.VerticalConstrainRaycastLayerMask)) {
				//if (hit.collider.GetComponent<Grid> ().playerId == client.connection.connectionId)
				transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
			}
		} else {
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.RaycastLayerMask)) {
				if (hit.collider.GetComponent<Grid>().playerId == client.connection.connectionId) {
					
					transform.position = hit.point;
				}
			}
		}
	}
	
	private void OnDrawGizmos() {
		Gizmos.DrawCube(transform.position, Vector3.one);
	}
}