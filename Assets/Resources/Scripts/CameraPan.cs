using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class CameraPan : MonoBehaviour {
	[Range(0, 8)]
	public float MovementSpeed = 1f;
	[Range(0, 10)]
	public float RotationSpeed = 4f;
	
	[Range(0, 50)]
	public float TiltHeight = 18f;
	[Range(0, 50)]
	public float FlatHeight = 3f;
	
	[Range(0, 90)]
	public float TiltAngle = 35f;
	[Range(0, 90)]
	public float FlatAngle = 20f;

	private Vector3 PivotPoint = Vector3.zero;
	
	void LateUpdate() {
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		flatForward.Normalize();
		Vector3 difference = MovementSpeed * (flatForward * Input.GetAxis("Vertical") +
			Vector3.Cross(flatForward, Vector3.up).normalized * -Input.GetAxis("Horizontal") +
			Vector3.up * Input.GetAxis("Up/Down"));
		transform.position += difference;
		PivotPoint += difference;
		
		transform.RotateAround(PivotPoint, Vector3.up, -Input.GetAxis("Rotate") * RotationSpeed);
		
		transform.rotation = Quaternion.Euler(
			Mathf.Lerp(FlatAngle, TiltAngle, Mathf.SmoothStep(0f, 1f, (transform.position.y - FlatHeight) / (TiltHeight - FlatHeight))),
			transform.eulerAngles.y,
			transform.eulerAngles.z
		);
	}
}
