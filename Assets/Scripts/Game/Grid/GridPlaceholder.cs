using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BattlePark {
	public class GridPlaceholder : MonoBehaviour {
		private new Camera camera;
		private Grid grid;
	
		public GridObject GridObject;
	
		public int Owner;
	
		private void Start() {
			camera = FindObjectOfType<Camera>();
			grid = FindObjectOfType<Grid>();
			GridObject = GetComponent<GridObject>();
		}
	
		public void Snap() {
			if (GridObject == null) {
				return;
			}
		
			transform.rotation = Quaternion.Euler(-90, 0, (int)GridObject.Direction * 90);
		
			transform.position = new Vector3 { //snap to grid
				x = (Mathf.Round((transform.position.x - 0.5f) / grid.GridStepXZ) * grid.GridStepXZ) + 0.5f,
				y = Mathf.Clamp(Mathf.Round(transform.position.y / grid.GridStepY) * grid.GridStepY, 0, float.PositiveInfinity),
				z = (Mathf.Round((transform.position.z - 0.5f) / grid.GridStepXZ) * grid.GridStepXZ) + 0.5f
			};
		}
	
		public void PlaceObject() {
			Vector3 snappedPos = new Vector3 {
				x = Mathf.Round((transform.position.x - 0.5f) / grid.GridStepXZ),
				y = Mathf.Clamp(Mathf.RoundToInt(transform.position.y / grid.GridStepY), 0, float.PositiveInfinity),
				z = Mathf.Round((transform.position.z - 0.5f) / grid.GridStepXZ)
			};
		
			if (grid.Objects.WillIntersect(snappedPos, GridObject.RotatedOffsets()) || !grid.ValidRegion(transform.position, 6666666)) {
				return;
			}
		
			GridObject.X = (int)snappedPos.x;
			GridObject.Y = (int)snappedPos.y;
			GridObject.Z = (int)snappedPos.z;
		
			//GRID PLACEHOLDER!!!!!!!!!!
		
			Destroy(gameObject);
		}
	
		public void Rotate(int direction) {
			GridObject.Direction += direction;
		
			if (GridObject.Direction > (Direction)3) {
				GridObject.Direction = (Direction)0;
			}
			if (GridObject.Direction < (Direction)0) {
				GridObject.Direction = (Direction)3;
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
				if (grid == null)
					return;
				if (hasHit = Physics.Raycast(camera.ScreenPointToRay(mousePosition), out hit, Mathf.Infinity, grid.RaycastLayerMask)) {
					transform.position = hit.point;
				}
			}
			gameObject.SetActive(hasHit);
		}
	}
}