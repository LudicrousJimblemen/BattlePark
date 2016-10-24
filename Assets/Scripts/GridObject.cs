using UnityEngine;

public class GridObject : MonoBehaviour
{
	private Grid grid;
	
	public bool IsBeingPlaced = true;
	
	void Start() {
		grid = FindObjectOfType<Grid>();
		GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
	}
	
	void Update() {
		if (IsBeingPlaced) {
			RaycastHit hit;
			
			if (Input.GetKey(KeyCode.LeftShift)) {
				if (Input.GetKeyDown(KeyCode.LeftShift)) {
					GameObject verticalConstraintPlane = (GameObject)Instantiate(
						grid.VerticalConstraintPrefab,
						transform.position,
						Quaternion.LookRotation(transform.position - FindObjectOfType<Camera>().transform.position) * Quaternion.Euler(-90, 0, 0)
					);
				}
				
				if (Physics.Raycast(FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.VerticalConstrainRaycastLayerMask)) {
					transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
				}
			} else {
				if (Physics.Raycast(FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, grid.RaycastLayerMask)) {
					transform.position = hit.point;
				}
			}
			
			if (Input.GetKeyUp(KeyCode.LeftShift)) {
				Destroy(FindObjectOfType<VerticalConstraint>().gameObject);
			}
			
			transform.position = new Vector3 { //snap to grid
				x = Mathf.Round(transform.position.x / grid.GridXZ) * grid.GridXZ,
				z = Mathf.Round(transform.position.z / grid.GridXZ) * grid.GridXZ,
				y = Mathf.Round(transform.position.y / grid.GridY) * grid.GridY
			};
		}
	}
}