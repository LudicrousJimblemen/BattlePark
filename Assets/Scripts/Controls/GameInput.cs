using UnityEngine;

/// <summary>
/// Controls input, and is the only class that recognizes input.
/// Any other class that recognizes input is illegal and should be wholly and immediately annihilated.
/// </summary>
public class GameInput : MonoBehaviour {
	public Transform VerticalConstraint;

	#region Debug
	Vector3? mousePosition = null;
	#endregion

	void Awake() {
		VerticalConstraint = Instantiate(VerticalConstraint, transform);
		VerticalConstraint.gameObject.SetActive(false);
	}

	void Update() {
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
		if (Input.GetMouseButtonDown(0) && mousePosition != null) {
			print("clik");
			GetComponent<Player>().CmdPlaceObject(mousePosition ?? default (Vector3), Quaternion.identity);
		}
	}
	void OnDrawGizmos() {
		Gizmos.DrawSphere(mousePosition ?? Vector3.zero, 0.1f);
	}
}
