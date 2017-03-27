using System;
using UnityEngine;

public class GridPlaceholder : MonoBehaviour {
	public GameObject VerticalConstraintPrefab;

	public Color ValidColor = Color.blue;
	public Color InvalidColor = Color.red;
	
	public Vector3[] PlaceholderOffsets;
	public GameObject VerticalConstraint;
	
	private GridObject currentReference;
	private int direction;
	
	private SkinnedMeshRenderer skinnedMeshRenderer;
	
	private void Awake() {
		skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		
		VerticalConstraint = Instantiate(VerticalConstraintPrefab, transform);
		VerticalConstraint.SetActive(false);
	}
	
	private void Update() {
		skinnedMeshRenderer.material.SetColor("_RimColor", valid ? ValidColor : InvalidColor);
	}
	
	public void Rotate(int counter) {
		direction += counter;
		if (direction > 3) {
			direction = 0;
		} else if (direction < 0) {
			direction = 3;
		}
		
		PlaceholderOffsets = currentReference.RotatedOffsets((Direction)direction);
	}
	
	public void Place() {
		
	}
	
	public void Reset(GridObject gridObject) {
		currentReference = gridObject;
		GetComponent<MeshFilter>().mesh = currentReference.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		GetComponent<SkinnedMeshRenderer>().sharedMesh = currentReference.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		
		PlaceholderOffsets = referr.RotatedOffsets((Direction)direction);
		
		if (!gridObject.CanRotate) {
			direction = 0;
		}
	}
	
	public void Disable() {
		direction = 0;
		PlaceholderOffsets = new [] { Vector3.zero };
	}
}