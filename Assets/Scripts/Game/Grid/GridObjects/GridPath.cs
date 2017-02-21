using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridPath : GridObject {
	public GameObject EdgeSouth;
	public GameObject EdgeNorth;
	public GameObject EdgeWest;
	public GameObject EdgeEast;
	public GameObject CornerSouthWest;
	public GameObject CornerSouthEast;
	public GameObject CornerNorthWest;
	public GameObject CornerNorthEast;

	public override Vector3[] OccupiedOffsets {
		get {
			return new[] { Vector3.zero };
		}
	}

	public override bool PlaceMultiple { get { return true; } }
	public override bool CanRotate { get { return false; } }

	public override void OnPlaced() {
		base.OnPlaced();

		UpdateMesh();
		
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				Vector3 nodePos = GetPosition() + new Vector3(-.75f, 0, -.75f) + new Vector3(i * 1.5f, 0, j * 1.5f);
				Instantiate(GameManager.Instance.PathNode, nodePos, Quaternion.identity, GameObject.FindGameObjectWithTag("PathNode").transform);
			}
		}
		AstarPath.active.Scan();
	}

	public void UpdateMesh() {
		foreach (var gridObject in Grid.Instance.Objects.AdjacentObjects(this.GridPosition, true)) {
			// "as" keyword returns null if cast is invalid
			GridPath path = gridObject as GridPath;
			if (path != null) {
				path.UpdateMesh();
			}
		}

		EdgeSouth.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(0, 0, -1)) as GridPath != null);
		EdgeNorth.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(0, 0, 1)) as GridPath != null);
		EdgeWest.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(-1, 0, 0)) as GridPath != null);
		EdgeEast.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(1, 0, 0)) as GridPath != null);

		CornerSouthWest.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(-1, 0, -1)) as GridPath != null);
		CornerSouthEast.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(1, 0, -1)) as GridPath != null);
		CornerNorthWest.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(-1, 0, 1)) as GridPath != null);
		CornerNorthEast.SetActive(Grid.Instance.Objects.ObjectAt(this.GridPosition + new Vector3(1, 0, 1)) as GridPath != null);
	}
}
