using System;
using UnityEngine;
using UnityEngine.Networking;

public class GridPlaceholder : MonoBehaviour
{
	private NetworkClient client;
	private new Camera camera;
	private Grid grid;
	private VerticalConstraint verticalConstraint;
	
	public GridObject GridObject;
	
	void Start()
	{
		client = FindObjectOfType<Client>().NetworkClient;
		camera = FindObjectOfType<Camera>();
		grid = FindObjectOfType<Grid>();
		verticalConstraint = FindObjectOfType<VerticalConstraint>();
		
		GridObject = GetComponent<GridObject>();
	}
	
	void Update()
	{
		verticalConstraint.MeshCollider.enabled = Input.GetKey(KeyCode.LeftControl);
		
		/*
		if (Input.GetKeyDown(KeyCode.A)) {
			if (GetComponent<Tree>()) {
				GetComponent<Tree>().SpinsALot = !GetComponent<Tree>().SpinsALot;
			}
		}
		 */
		
		transform.rotation = Quaternion.Euler(-90, 0, (int)GridObject.Direction * 90);
		
		transform.position = new Vector3 { //snap to grid
			x = Mathf.Floor(transform.position.x / grid.GridXZ) * grid.GridXZ + 0.5f * grid.GridXZ,
			z = Mathf.Floor(transform.position.z / grid.GridXZ) * grid.GridXZ + 0.5f * grid.GridXZ,
			y = Mathf.Clamp(Mathf.Round(transform.position.y / grid.GridY) * grid.GridY, 0, Mathf.Infinity)
		};
	}
	
	public void EnableVerticalConstraint()
	{
		Vector3 correctedPosition = new Vector3(
			                            camera.transform.position.x,
			                            0,
			                            camera.transform.position.z
		                            );
		verticalConstraint.transform.position = transform.position;
		verticalConstraint.transform.rotation = Quaternion.LookRotation(transform.position - correctedPosition) * Quaternion.Euler(-90, 0, 0);
	}
	
	public void PlaceObject()
	{
		client.Send(GridObjectPlacedNetMessage.Code, new GridObjectPlacedNetMessage() {
			ConnectionId = client.connection.connectionId,
			//N A M E ( C L O N E )
			//0 1 2 3 4 5 6 7 8 9 10
			Type = name.Substring(0, name.Length - 7),
			Position = transform.position,
			ObjectData = GridObject.Serialize()
		});
		FindObjectOfType<Client>().AllowSummons();
		Destroy(gameObject);
	}
	
	public void Rotate(int direction)
	{
		GridObject.Direction += direction;
		
		if (GridObject.Direction > (Direction)3) {
			GridObject.Direction = (Direction)0;
		}
		if (GridObject.Direction < (Direction)0) {
			GridObject.Direction = (Direction)3;
		}
	}
	
	public void Raycast(bool UseVerticalConstraint = false)
	{
		RaycastHit hit;
		if (UseVerticalConstraint) {
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.VerticalConstrainRaycastLayerMask)) {
				//if (hit.collider.GetComponent<Grid> ().playerId == client.connection.connectionId)
				transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
			}
		} else {
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.RaycastLayerMask)) {
				if (hit.collider.GetComponent<Grid>().playerId == client.connection.connectionId)
					transform.position = hit.point;
			}
		}
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.DrawCube(transform.position, Vector3.one);
	}
}