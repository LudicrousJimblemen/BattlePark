using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour {
	public Transform Target;
	
	private float isometric;
	private float height;
	
	private void Start() {
		isometric = Mathf.Atan2(1, Mathf.Sqrt(2));
		height = Mathf.Sqrt(32) / Mathf.Tan(isometric);
		
		transform.rotation = Quaternion.Euler(Mathf.Rad2Deg * isometric, 45, 0);
	}
	
	private void LateUpdate() {
		transform.position = Target.position + new Vector3(-4, height, -4);
	}
}
