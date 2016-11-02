using UnityEngine;
using System.Collections;

//Hopefully, this will be usable in any future project we do

[RequireComponent (typeof (Camera))]
public class CameraMovement : MonoBehaviour {
	
	public float speed = 5f;
	
	void LateUpdate () {
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		flatForward.Normalize ();
		transform.position += 
			speed * (flatForward * Input.GetAxis ("Vertical") + 
			Vector3.Cross (flatForward, Vector3.up).normalized * -Input.GetAxis ("Horizontal"));
		transform.Rotate (0, Input.GetAxis ("Mouse X") * speed * 3, 0, Space.World);
		transform.Rotate (Vector3.Cross (flatForward, Vector3.up).normalized, Input.GetAxis ("Mouse Y") * speed * 3, Space.World);
	}
}
