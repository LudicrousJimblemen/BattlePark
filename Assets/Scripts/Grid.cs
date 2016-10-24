using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	public GameObject VerticalConstraintPrefab;
	public GameObject ObjectPrefab;
	
	public float GridXZ = 1f;
	public float GridY = 0.5f;
	
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			FindObjectOfType<GridObject>().IsBeingPlaced = false;
		}
		
		if (Input.GetMouseButtonDown(1)) {
			Instantiate(ObjectPrefab, Vector3.zero, Quaternion.identity);
		}
	}
}