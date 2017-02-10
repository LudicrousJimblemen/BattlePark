using UnityEngine;

/// <summary>
/// Controls input, and is the only class that recognizes input.
/// Any other class that recognizes input is illegal and should be wholly and immediately annihilated.
/// </summary>
public class GameInput : MonoBehaviour {
	public Transform VerticalConstraint;
	public MeshFilter Placeholder;

	#region Debug
	Vector3? mousePosition = null;
	#endregion
	
	Player player;
	
	int hotbarIndex = -1;
	int direction;

	void Awake() {
		VerticalConstraint = Instantiate(VerticalConstraint, transform);
		VerticalConstraint.gameObject.SetActive(false);
		Placeholder = Instantiate(Placeholder) as MeshFilter;
		player = GetComponent<Player> ();
	}

	void Update() {
		if (!player.isLocalPlayer) 
			return;
		for (int i = 0; i < 9; i++) {
			if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
				hotbarIndex = i;
				Placeholder.mesh = player.GridObjects[i].GetComponent<MeshFilter> ().sharedMesh;
				break;
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			hotbarIndex = -1;
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			direction ++;
			if (direction > 3) {
				direction = 0;
			}
		}
		Placeholder.gameObject.SetActive (hotbarIndex != -1);
		bool verticalConstraint = Input.GetKey(KeyCode.LeftControl);
		VerticalConstraint.gameObject.SetActive(verticalConstraint);
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, verticalConstraint ? Grid.Instance.VerticalConstraintRaycastLayerMask : Grid.Instance.RaycastLayerMask)) {
			Vector3 constraintPos = mousePosition ?? Vector3.zero;
			if (VerticalConstraint) {
				constraintPos.y = hit.point.y;
			}
			mousePosition = Grid.Instance.SnapToGrid(verticalConstraint ? constraintPos : hit.point, 1);
		} else {
			mousePosition = null;
		}
		if (!verticalConstraint) {
			Vector3 corrected = mousePosition ?? Vector3.zero;
			corrected.y = 0;
			Vector3 correctedCam = Camera.main.transform.position;
			correctedCam.y = 0;
			VerticalConstraint.position = corrected;
			VerticalConstraint.rotation = Quaternion.LookRotation(correctedCam - corrected) * Quaternion.Euler(90, 0, 0);
		}
		if (mousePosition != null) {
			Placeholder.transform.position = mousePosition ?? Vector3.zero;
			Placeholder.transform.rotation = Quaternion.Euler(-90,0,(int)direction * 90);
		} else {
			Placeholder.gameObject.SetActive (false);
		}
		if (Input.GetMouseButtonDown(0)) {
			print("clik");
			if (mousePosition != null && hotbarIndex != -1) {
				GetComponent<Player>().CmdPlaceObject(hotbarIndex, mousePosition ?? Vector3.zero, direction);
				if (!player.GridObjects[hotbarIndex].PlaceMultiple)
					hotbarIndex = -1;
			}
		}
	}
	void OnDrawGizmos() {
		Gizmos.DrawSphere(mousePosition ?? Vector3.zero, 0.1f);
	}
}
