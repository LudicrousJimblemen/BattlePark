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
	
	[Range(0, 5)]
	public float MinimumPivotY = 1f;

	//the point which the camera pivots around
	//wow
	private Vector3 PivotPoint = Vector3.zero;
	
	void LateUpdate() {
		//Stores the normalized direction of the camera with the y set to 0
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		flatForward.Normalize();
		
		//Stores a vector with the combined inputs of all 6 cartesian directions
		Vector3 difference = MovementSpeed * (flatForward * Input.GetAxis("Vertical") +
			Vector3.Cross(flatForward, Vector3.up).normalized * -Input.GetAxis("Horizontal") +
			Vector3.up * Input.GetAxis("Up/Down"));
		
		//Stops difference from taking the pivotpoint below minimum
		if (PivotPoint.y + difference.y < MinimumPivotY) {
			difference.y = 0; 
		}
		
		//Applies difference vector to transform and pivot
		transform.position += difference;
		PivotPoint += difference;
		
		//Control rotation around the y axis		
		transform.RotateAround(PivotPoint, Vector3.up, Input.GetAxis("Rotate") * RotationSpeed);
		
		//Rotate around the local x axis of camera going through the pivot, based on the height of the camera
		//ie: Rotate downwards harsher at higher elevations
		transform.rotation = Quaternion.Euler(
			Mathf.Lerp(FlatAngle, TiltAngle, Mathf.SmoothStep(0f, 1f, (transform.position.y - FlatHeight) / (TiltHeight - FlatHeight))),
			transform.eulerAngles.y,
			transform.eulerAngles.z
		);
	}
}
