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

	public override int Cost { get { return 1000; } }

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
		foreach (var gridObject in Grid.Instance.Objects.AdjacentObjects(this.GridPosition, true)) {
			// "as" keyword returns null if cast is invalid
			GridPath path = gridObject as GridPath;
			if (path != null) {
				path.UpdateMesh();
			}
		}
		
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				Vector3 nodePos = GetPosition() + new Vector3(-.75f, 0, -.75f) + new Vector3(i * 1.5f, 0, j * 1.5f);
				Instantiate(GameManager.Instance.PathNode, nodePos, Quaternion.identity, GameObject.FindGameObjectWithTag("PathNode").transform);
			}
		}
		AstarPath.active.Scan();
	}

	public void UpdateMesh() {
		GridObject[] adj = Grid.Instance.Objects.AdjacentObjects(GetPosition(),true);
        EdgeEast.SetActive(adj[0] as GridPath != null);
		EdgeWest.SetActive(adj[1] as GridPath != null);
		EdgeNorth.SetActive(adj[2] as GridPath != null);
		EdgeSouth.SetActive(adj[3] as GridPath != null);

		CornerNorthEast.SetActive(adj[4] as GridPath != null);
		CornerSouthWest.SetActive(adj[5] as GridPath != null);
		CornerNorthWest.SetActive(adj[6] as GridPath != null);
		CornerSouthEast.SetActive(adj[7] as GridPath != null);
	}
}
