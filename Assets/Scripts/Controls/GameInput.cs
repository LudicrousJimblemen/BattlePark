using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Controls input, and is the only class that recognizes input.
/// Any other class that recognizes input is illegal and should be wholly and immediately annihilated.
/// </summary>
public class GameInput : MonoBehaviour {
	public Transform VerticalConstraint;
	public MeshFilter Placeholder;
	public GameObject PlaceholderCameraPrefab;
	
	private GridObject placeholderGridObject;
	private Vector3[] placeholderOffsets;

	Vector3? mousePosition;
	Vector3 rawMouse;

	Player player;

	int hotbarIndex = -1;
	int direction;

	public Color ValidColor = Color.blue;
	public Color InvalidColor = Color.red;

	private void Start() {
		player = GetComponent<Player>();
		if (!player.isLocalPlayer)
			return;
		VerticalConstraint = Instantiate(VerticalConstraint, transform);
		VerticalConstraint.gameObject.SetActive(false);
		Placeholder = Instantiate(Placeholder) as MeshFilter;
		placeholderOffsets = new [] { Vector3.zero };
	}

	private void Update() {
		if (!player.isLocalPlayer)
			return;
		for (int i = 0; i < 9; i++) {
			if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
				if (player.ObjectIndices[i] == -1)
					continue;
				hotbarIndex = i;
				placeholderGridObject = GameManager.Instance.Objects[player.ObjectIndices[i]];
				Placeholder.mesh = placeholderGridObject.GetComponent<MeshFilter>().sharedMesh;
				Placeholder.GetComponent<SkinnedMeshRenderer>().sharedMesh = Placeholder.mesh;

				GameGUI.Instance.UpdatePlaceholderWindow(placeholderGridObject,Placeholder.transform);
				
				placeholderOffsets = placeholderGridObject.RotatedOffsets((Direction)direction);
				break;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			direction = 0;
			hotbarIndex = -1;
			GameGUI.Instance.UpdatePlaceholderWindow(null, null);
			placeholderOffsets = new [] { Vector3.zero };
		}
		
		GridOverlay.Instance.ShowGrid = hotbarIndex != -1;
		
		Placeholder.gameObject.SetActive(hotbarIndex != -1);
		bool verticalConstraint = Input.GetKey(KeyCode.LeftControl);
		VerticalConstraint.gameObject.SetActive(verticalConstraint);
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, verticalConstraint ? Grid.Instance.VerticalConstraintRaycastLayerMask : Grid.Instance.RaycastLayerMask)) {
			Vector3 constraintPos = mousePosition ?? Vector3.zero;
			if (VerticalConstraint) {
				constraintPos.y = hit.point.y;
			}
			mousePosition = verticalConstraint ? constraintPos : hit.point;
			rawMouse = mousePosition.Value;
			mousePosition = Grid.Instance.SnapToGrid(mousePosition.Value, player.PlayerNumber);
		} else {
			mousePosition = null;
			Placeholder.gameObject.SetActive(false);
		}
		if (hotbarIndex != -1) {
			if (Input.GetKeyDown(KeyCode.R) && placeholderGridObject.CanRotate) {
				int counter = Input.GetKey(KeyCode.LeftShift) ? -1 : 1;
				direction += counter;
				if (direction > 3) {
					direction = 0;
				} else if (direction < 0) {
					direction = 3;
				}
				placeholderOffsets = placeholderGridObject.RotatedOffsets((Direction)direction);
			}
			if (!verticalConstraint) {
				Vector3 corrected = mousePosition ?? Vector3.zero;
				corrected.y = 0;
				Vector3 correctedCam = Camera.main.transform.position;
				correctedCam.y = 0;
				VerticalConstraint.position = corrected;
				VerticalConstraint.rotation = Quaternion.LookRotation(correctedCam - corrected) * Quaternion.Euler(90, 0, 0);
			}
			if (mousePosition != null) {
				Placeholder.transform.position = mousePosition.Value;
				Placeholder.transform.rotation = Quaternion.Euler(-90, 0, (int)direction * 90);
			} else {
				Placeholder.gameObject.SetActive(false);
			}

			bool valid = mousePosition != null 
				&& player.getObject(hotbarIndex).Valid(mousePosition.Value, (Direction)direction, player.PlayerNumber) 
				&& player.Money >= placeholderGridObject.Cost;

			Placeholder.GetComponent<SkinnedMeshRenderer>().material.SetColor("_RimColor", valid ? ValidColor : InvalidColor);
			if (Input.GetMouseButtonDown(0)) {
				if (valid) {
					if (mousePosition != null && hotbarIndex != -1) {
						player.PlaceObject(hotbarIndex, mousePosition, placeholderGridObject.CanRotate ? direction : 0);
						if (!GameManager.Instance.Objects[player.ObjectIndices[hotbarIndex]].PlaceMultiple) {
							hotbarIndex = -1;
							GameGUI.Instance.UpdatePlaceholderWindow(null,null);
							placeholderOffsets = new [] { Vector3.zero };
							/*
							if (WindowManager.Instance.Windows.Any(w => w.Type == WindowType.Placeholder)) {
								WindowManager.Instance.Windows.Remove(WindowManager.Instance.Windows.First(w => w.Type == WindowType.Placeholder));
							}
							*/
						}
					}
				}
			}
		}
		if (Input.GetMouseButtonDown(1)) {
			player.CmdSpawnPerson(rawMouse, player.PlayerNumber);
		}
	}

	private void OnDrawGizmos() {
		if (Placeholder.gameObject.activeInHierarchy) {
			Gizmos.color = Color.red;
			foreach (Vector3 offset in placeholderOffsets) {
				Gizmos.DrawSphere(offset + mousePosition.Value, 0.2f);
			}
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(rawMouse, 0.2f);
	}
}
