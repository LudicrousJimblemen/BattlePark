﻿using UnityEngine;

/// <summary>
/// Controls input, and is the only class that recognizes input.
/// Any other class that recognizes input is illegal and should be wholly and immediately annihilated.
/// </summary>
public class GameInput : MonoBehaviour {
	public Transform VerticalConstraint;
	public MeshFilter Placeholder;
	private GridObject placeholderGridObj;
	private Vector3[] placeholderOffsets;
	
	Vector3? mousePosition = null;
	
	Player player;
	
	int hotbarIndex = -1;
	int direction;

	public Color ValidColor = Color.blue;
	public Color InvalidColor = Color.red;

	void Start() {
		player = GetComponent<Player> ();
		if (!player.isLocalPlayer)
			return;
		VerticalConstraint = Instantiate(VerticalConstraint, transform);
		VerticalConstraint.gameObject.SetActive(false);
		Placeholder = Instantiate(Placeholder) as MeshFilter;
		placeholderOffsets = new Vector3[] { Vector3.zero };
	}

	void Update() {
		if (!player.isLocalPlayer) 
			return;
        for (int i = 0; i < 9; i++) {
			if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
				//print (player.ObjectIndices[0]);
				if (player.ObjectIndices[i] == -1) 
					continue;
				hotbarIndex = i;
				placeholderGridObj = GameManager.Instance.Objects[player.ObjectIndices[i]];
                Placeholder.mesh = placeholderGridObj.GetComponent<MeshFilter> ().sharedMesh;
				placeholderOffsets = placeholderGridObj.RotatedOffsets((Direction)direction);
				break;
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			hotbarIndex = -1;
			placeholderOffsets = new Vector3[] { Vector3.zero };
		}
		Placeholder.gameObject.SetActive (hotbarIndex != -1);
		if(hotbarIndex != -1) {
			if (Input.GetKeyDown (KeyCode.R)) {
				int counter = Input.GetKey(KeyCode.LeftShift) ? -1 : 1;
				direction += counter;
				if (direction > 3) {
					direction = 0;
				} else if (direction < 0) {
					direction = 3;
				}
				placeholderOffsets = placeholderGridObj.RotatedOffsets((Direction)direction);
			}
			bool verticalConstraint = Input.GetKey(KeyCode.LeftControl);
			VerticalConstraint.gameObject.SetActive(verticalConstraint);
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, verticalConstraint ? Grid.Instance.VerticalConstraintRaycastLayerMask : Grid.Instance.RaycastLayerMask)) {
				Vector3 constraintPos = mousePosition ?? Vector3.zero;
				if (VerticalConstraint) {
					constraintPos.y = hit.point.y;
				}
				mousePosition = Grid.Instance.SnapToGrid(verticalConstraint ? constraintPos : hit.point, 1);
			} else {
				mousePosition = null;
				Placeholder.gameObject.SetActive(false);
			}
			if (!verticalConstraint) {
				Vector3 corrected = mousePosition ?? Vector3.zero;
				corrected.y = 0;
				Vector3 correctedCam = Camera.main.transform.position;
				correctedCam.y = 0;
				VerticalConstraint.position = corrected;
				VerticalConstraint.rotation = Quaternion.LookRotation(correctedCam - corrected) * Quaternion.Euler(90, 0, 0);
			}
			if(mousePosition != null) {
				Placeholder.transform.position = mousePosition.Value;
				Placeholder.transform.rotation = Quaternion.Euler(-90,0,(int)direction * 90);
			} else {
				Placeholder.gameObject.SetActive(false);
			}
			Placeholder.GetComponent<MeshRenderer>().material.SetColor("_RimColor",Grid.Instance.Objects.WillIntersect(mousePosition.Value,placeholderOffsets) ? InvalidColor : ValidColor);
			if (Input.GetMouseButtonDown(0)) {
				//print (hotbarIndex);
				//print (mousePosition.ToString ());
				if (mousePosition != null && hotbarIndex != -1) {
					//print ("cool");
					player.PlaceObject(hotbarIndex, mousePosition, direction);
					if(!GameManager.Instance.Objects[player.ObjectIndices[hotbarIndex]].PlaceMultiple) {
						hotbarIndex = -1;
						placeholderOffsets = new Vector3[] { Vector3.zero };
					}
				}
			}
		}
	}
	void OnDrawGizmos() {
		if (Placeholder.gameObject.activeInHierarchy) {
			Gizmos.color = Color.red;
			foreach(Vector3 offset in placeholderOffsets) {
				Gizmos.DrawSphere(offset + mousePosition.Value,0.2f);
			}
		}
	}
}
