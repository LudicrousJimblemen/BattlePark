/* Class for generic input handling; everything input-related outside of gui goes here
 *
 * Please do not make other classes recognize inputs; thanks, future me (us)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

	public Transform VerticalConstraint;

	#region debug
	Vector3? mousePosition = null;
	#endregion

	void Awake () {
		VerticalConstraint = Instantiate(VerticalConstraint,transform);
		VerticalConstraint.gameObject.SetActive(false);
	}

	void Update () {
		bool verticalConstraint = Input.GetKey(KeyCode.LeftControl);
        VerticalConstraint.gameObject.SetActive(verticalConstraint);
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, Mathf.Infinity, verticalConstraint ? Grid.Instance.VerticalConstraintRaycastLayerMask : Grid.Instance.RaycastLayerMask)) {
			Vector3 constraintPos = mousePosition ?? default(Vector3);
			if (VerticalConstraint) {
				constraintPos.y = hit.point.y;
			}
			mousePosition = Grid.Instance.SnapToGrid (verticalConstraint ? constraintPos : hit.point, 1);
		} else {
			mousePosition = null;
		}
		if (!verticalConstraint) {
			Vector3 corrected = mousePosition ?? default(Vector3);
			corrected.y = 0;
			Vector3 correctedCam = Camera.main.transform.position;
			correctedCam.y = 0;
			VerticalConstraint.position = corrected;
			VerticalConstraint.rotation = Quaternion.LookRotation(correctedCam - corrected) * Quaternion.Euler(90,0,0);
		}
		if (Input.GetMouseButtonDown (0) && mousePosition != null) {
			print ("clik");
			GetComponent<Player> ().CmdPlaceObject (mousePosition ?? default (Vector3), Quaternion.identity);
		}
	}
	void OnDrawGizmos () {
		Gizmos.DrawSphere(mousePosition ?? default(Vector3), 0.1f); //if null, return default; else, return vector
	}
}
