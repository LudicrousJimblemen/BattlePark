using UnityEngine;
using System;

/// <summary>
/// Follows an object with an isometric perspective.
/// </summary>
[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour {
	public Transform Target;
	public RenderTexture Output { get { return GetComponent<Camera>().targetTexture; } set { GetComponent<Camera>().targetTexture = value; } }
	
	private void Start() {
		transform.rotation = Quaternion.Euler(Mathf.Rad2Deg * Mathf.Atan(1 / Mathf.Sqrt(2)), 45, 0);
	}
	
	private void LateUpdate() {
		transform.position = Target.position + new Vector3(-16, 16, -16);
	}
}
