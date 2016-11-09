using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	
	public int playerId;
	
	public float GridXZ = 1f;
	public float GridY = 0.5f;
	
	void Start()
	{
		// G R I D # # #
		// 0 1 2 3 4 5 6 etc
		playerId = int.Parse(name.Substring(4, name.Length - 4));
	}
}