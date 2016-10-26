using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
	private NetworkManager networkManager;
	
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	public GameObject PlaceholderObjectPrefab;
	public GameObject GridObjectPrefab;
	
	public float GridXZ = 1f;
	public float GridY = 0.5f;
	
	void Start() {
		networkManager = FindObjectOfType<NetworkManager>();
	}
}