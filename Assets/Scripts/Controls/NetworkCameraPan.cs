using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// Just a clone of <see cref="CameraPan"/>, but designed to control input of the camera when it's the parent of the player.
/// </summary>
public class NetworkCameraPan : NetworkBehaviour {
	public bool Enabled = true;

	[Range(0, 8)]
	public float MovementSpeed = 1f;
	[Range(0, 10)]
	public float RotationSpeed = 4f;

	[Range(0, 50)]
	public float TiltHeight = 36f;
	[Range(0, 50)]
	public float FlatHeight = 3f;

	[Range(0, 90)]
	public float TiltAngle = 45f;
	[Range(0, 90)]
	public float FlatAngle = 20f;

	[Range(-5, 10)]
	public float MinimumPivotY = 3f;

	private Vector3 PivotPoint = Vector3.zero;

	void LateUpdate() {
		// Stores the normalized direction of the camera with the y set to 0
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		flatForward.Normalize();

		Vector3 difference = Vector3.zero;
		if (Enabled) {
			// Stores a vector with the combined inputs of all 6 cartesian directions
			difference = MovementSpeed * (flatForward * Input.GetAxis("Vertical") +
			Vector3.Cross(flatForward, Vector3.up).normalized * -Input.GetAxis("Horizontal") +
			Vector3.up * Input.GetAxis("Up/Down"));
		}
		// Applies difference vector to transform and pivot
		transform.position += difference;
		if (transform.position.y < MinimumPivotY)
			transform.position = new Vector3(transform.position.x, MinimumPivotY, transform.position.z);
		// Control rotation around the y axis
		PivotPoint = GetPivot();
		if (Enabled) {
			transform.RotateAround(PivotPoint, Vector3.up, Input.GetAxis("Rotate") * RotationSpeed);
		}

		// Rotate around the local x axis of camera going through the pivot, based on the height of the camera
		// i.e. Rotate downwards harsher at higher elevations
		transform.rotation = Quaternion.Euler(
			Mathf.Lerp(FlatAngle, TiltAngle, Mathf.SmoothStep(0f, 1f, (transform.position.y - FlatHeight) / (TiltHeight - FlatHeight))),
			transform.eulerAngles.y,
			transform.eulerAngles.z
		);
	}
	Vector3 GetPivot() {
		float angle = Mathf.Acos(Vector3.Dot(Vector3.down, transform.forward)); // angle between down and forwards directions
		if (Mathf.PI / 2 - angle == 0)
			return Vector3.zero;
		// print(angle);
		float y = transform.position.y;
		float flatDistToPivot = angle * y / (Mathf.PI / 2 - angle);
		Vector3 flatForward = transform.forward;
		flatForward.y = 0;
		Vector3 flatPos = transform.position;
		flatPos.y = 0;
		return flatPos + flatForward.normalized * flatDistToPivot;
	}
}