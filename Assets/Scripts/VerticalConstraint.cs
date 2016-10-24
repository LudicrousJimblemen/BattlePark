using UnityEngine;

public class VerticalConstraint : MonoBehaviour { 
	[HideInInspector]
	public MeshCollider MeshCollider;
	
	void Start() {
		MeshCollider = GetComponent<MeshCollider>();
	}
}