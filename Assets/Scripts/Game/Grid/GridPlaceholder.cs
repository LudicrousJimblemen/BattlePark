using System;
using System.Linq;
using UnityEngine;

namespace BattlePark {
	public class GridPlaceholder : MonoBehaviour {
		private Camera camera;
		
		private Client client;
		
		private Grid grid;
		private GridObject gridObject;
	
		private void Awake() {
			camera = FindObjectOfType<Camera>();
			client = FindObjectOfType<Client>();
			grid = FindObjectOfType<Grid>();
			gridObject = FindObjectOfType<GridObject>();
		}
	
		public void Snap() {
			transform.rotation = Quaternion.Euler(-90, 0, (int)gridObject.Direction * 90);
		
			transform.position = new Vector3 { //snap to grid
				x = (Mathf.Round((transform.position.x - 0.5f) / grid.GridStepXZ) * grid.GridStepXZ) + 0.5f,
				y = Mathf.Clamp(Mathf.Round(transform.position.y / grid.GridStepY) * grid.GridStepY, 0, float.PositiveInfinity),
				z = (Mathf.Round((transform.position.z - 0.5f) / grid.GridStepXZ) * grid.GridStepXZ) + 0.5f
			};
		}
	
		public void Rotate(int direction) {
			gridObject.Direction += direction;
		
			if (gridObject.Direction > (Direction)3) {
				gridObject.Direction = (Direction)0;
			}
			if (gridObject.Direction < (Direction)0) {
				gridObject.Direction = (Direction)3;
			}
		}
	
		public void Position(Vector3 mousePosition, bool UseVerticalConstraint = false) {
			RaycastHit hit;
			bool hasHit;
			if (UseVerticalConstraint) {
				if (hasHit = Physics.Raycast(camera.ScreenPointToRay(mousePosition), out hit, Mathf.Infinity, grid.VerticalConstrainRaycastLayerMask)) {
					transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
				}
			} else {
				if (hasHit = Physics.Raycast(camera.ScreenPointToRay(mousePosition), out hit, Mathf.Infinity, grid.RaycastLayerMask)) {
					transform.position = hit.point;
				}
			}
			gameObject.SetActive(hasHit);
		}
	
		public void PlaceObject() {
			Vector3 snappedPos = new Vector3 {
				x = Mathf.Round((transform.position.x - 0.5f) / grid.GridStepXZ),
				y = Mathf.Clamp(Mathf.RoundToInt(transform.position.y / grid.GridStepY), 0, float.PositiveInfinity),
				z = Mathf.Round((transform.position.z - 0.5f) / grid.GridStepXZ)
			};
		
			if (grid.Objects.WillIntersect(snappedPos, gridObject.RotatedOffsets()) || !grid.ValidRegion(transform.position, client.GetUniqueId())) {
				return;
			}
		
			gridObject.X = (int)snappedPos.x;
			gridObject.Y = (int)snappedPos.y;
			gridObject.Z = (int)snappedPos.z;
		
			//GRID PLACEHOLDER MESSAGE!!!!!!!!!!!!
		
			Destroy(gameObject);
		}
	}
}