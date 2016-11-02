using UnityEngine;
using System.Collections;

//Hopefully, this will be usable in any future project we do

[RequireComponent (typeof (Camera))]
public class CameraMovement : MonoBehaviour {

	public float speed = 5f;
	public float boundsRadius = 150f;
	public Vector3 boundsCenter = Vector3.zero;
	
	void LateUpdate () {
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		flatForward.Normalize ();
		Vector3 difference = speed * (flatForward * Input.GetAxis ("Vertical") +
			Vector3.Cross (flatForward,Vector3.up).normalized * -Input.GetAxis ("Horizontal") + 
			Vector3.up * Input.GetAxis ("Up/Down"));

		Vector3 clampedPos;

		if (Vector3.Distance (transform.position + difference,boundsCenter) > Mathf.Abs (boundsRadius) && boundsRadius != 0) {
			clampedPos = (transform.position + difference).normalized * Mathf.Abs (boundsRadius);
		} else {
			clampedPos = transform.position + difference;
        }

		transform.position = clampedPos;


		transform.Rotate (0, Input.GetAxis ("Mouse X") * speed * 3, 0, Space.World);
		transform.Rotate (Vector3.Cross (flatForward, Vector3.up).normalized, Input.GetAxis ("Mouse Y") * speed * 3, Space.World);

		transform.rotation = Quaternion.LookRotation (transform.forward,Vector3.up);
	}
}
