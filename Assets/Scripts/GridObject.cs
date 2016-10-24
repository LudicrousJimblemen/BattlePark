using UnityEngine;

public class GridObject : MonoBehaviour
{
	private Camera camera;
	private Grid grid;
	private VerticalConstraint verticalConstraint;
	
	public bool IsBeingPlaced = true;
	
	void Start() {
		camera = FindObjectOfType<Camera>();
		grid = FindObjectOfType<Grid>();
		verticalConstraint = FindObjectOfType<VerticalConstraint>();
		GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
	}
	
	void Update() {
		if (IsBeingPlaced) {
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
		}
	}
}