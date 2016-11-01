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
	public string Type;
	
	void Start() {
		client = FindObjectOfType<Client>().NetworkClient;
		camera = FindObjectOfType<Camera>();
		grid = FindObjectOfType<Grid>();
		verticalConstraint = FindObjectOfType<VerticalConstraint>();
		
		GridObject = GetComponent<GridObject>();
	}
	
	void Update() {
		verticalConstraint.MeshCollider.enabled = Input.GetKey(KeyCode.LeftShift);
			
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			Vector3 correctedPosition = new Vector3(
				camera.transform.position.x,
				0,
				camera.transform.position.z
			);
			verticalConstraint.transform.position = transform.position;
			verticalConstraint.transform.rotation = Quaternion.LookRotation(transform.position - correctedPosition) * Quaternion.Euler(-90, 0, 0);
		}
		
		if (Input.GetKeyDown(KeyCode.A)) {
			if (GetComponent<Tree>()) {
				GetComponent<Tree>().SpinsALot = !GetComponent<Tree>().SpinsALot;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Z)) {
			GridObject.Direction--;
		} else if (Input.GetKeyDown(KeyCode.X)) {
			GridObject.Direction++;
		}
		
		if (GridObject.Direction > (Direction)3) {
			GridObject.Direction = (Direction)0;
		}
		if (GridObject.Direction < (Direction)0) {
			GridObject.Direction = (Direction)3;
		}
		
		transform.rotation = Quaternion.Euler(-90, 0, (int)GridObject.Direction * 90);
		
		RaycastHit hit;
		
		if (Input.GetKey(KeyCode.LeftShift)) {
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.VerticalConstrainRaycastLayerMask)) {
				transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
			}
		} else {
			if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.RaycastLayerMask)) {
				transform.position = hit.point;
			}
		}
		
		transform.position = new Vector3 { //snap to grid
			x = Mathf.Round(transform.position.x / grid.GridXZ) * grid.GridXZ,
			z = Mathf.Round(transform.position.z / grid.GridXZ) * grid.GridXZ,
			y = Mathf.Clamp(Mathf.Round(transform.position.y / grid.GridY) * grid.GridY, 0, Mathf.Infinity)
		};
		
		if (Input.GetMouseButtonDown(0)) {
			client.Send(GridObjectPlacedNetMessage.Code, new GridObjectPlacedNetMessage() {
				ConnectionId = client.connection.connectionId,
				
				Type = Type,
				
				Position = transform.position,
				ObjectData = GridObject.Serialize()
			});
			Destroy(gameObject);
		}
	}
	void OnDrawGizmos() {
		Gizmos.DrawCube(transform.position, Vector3.one);
	}
}