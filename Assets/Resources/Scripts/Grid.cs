using System;
using UnityEngine;

public class Grid : MonoBehaviour {
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	
	public float GridXZ = 1f;
	public float GridY = 0.5f;
}