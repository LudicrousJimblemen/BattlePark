using UnityEngine;
using System.Collections;

//Hopefully, this will be usable in any future project we do

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour {

	public float speed = 5f;
	[Range(0, Mathf.Infinity)]
	public float boundsRadius = 150f;
	public Vector3 boundsCenter = Vector3.zero;
	[Range(0, Mathf.Infinity)]
	public float minimumHeight = 5f;
	
	void LateUpdate() {
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		flatForward.Normalize();
		Vector3 difference = speed * (flatForward * Input.GetAxis("Vertical") +
		                     Vector3.Cross(flatForward, Vector3.up).normalized * -Input.GetAxis("Horizontal") +
		                     Vector3.up * Input.GetAxis("Up/Down"));

		Vector3 clampedPos;
		if (boundsRadius != 0) {
			clampedPos = transform.position + difference;
			if (Vector3.Distance(transform.position + difference, boundsCenter) > Mathf.Abs(boundsRadius)) {
				clampedPos = (transform.position + difference).normalized * Mathf.Abs(boundsRadius);
			} 
			if ((transform.position + difference).y < minimumHeight) {
				clampedPos.y = minimumHeight;
			}
		} else {
			clampedPos = transform.position + difference;
		} 

		transform.position = clampedPos;

		if (Input.GetMouseButton(1)) {
			transform.Rotate(0, Input.GetAxis("Mouse X") * speed * 3, 0, Space.World);
			transform.Rotate(Vector3.Cross(flatForward, Vector3.up).normalized, Input.GetAxis("Mouse Y") * speed * 3, Space.World);
	
			transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
		}
		
	}
}
