using System;
using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	
	public int PlayerId;
	
	public float GridXZ = 1f;
	public float GridY = 0.5f;
	
	public Dictionary<Vector3, GridObject> Objects;
	
	void Start() {
		// G R I D # # #
		// 0 1 2 3 4 5 6 etc
		PlayerId = int.Parse(name.Substring(4, name.Length - 4));
		Objects = new Dictionary<Vector3, GridObject> ();
	}
}