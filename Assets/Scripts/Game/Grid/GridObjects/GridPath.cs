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
	public override void OnPlaced() {
		base.OnPlaced();

		UpdateMesh();
		foreach (var gridObject in Grid.Instance.Objects.AdjacentObjects(GetPosition(), true)) {
			// "as" keyword returns null if cast is invalid
			GridPath path = gridObject as GridPath;
			if (path != null) {
				path.UpdateMesh();
			}
		}
		
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				Vector3 nodePos = GetPosition() + new Vector3(-1, 0, -1) + new Vector3(i, 0, j);
				Instantiate(GameManager.Instance.PathNode, nodePos, Quaternion.identity, GameObject.FindGameObjectWithTag("PathNode").transform);
			}
		}
		AstarPath.active.Scan ();
	}

	public override bool CanRotate { get { return false; } }

	public void UpdateMesh() {
		GridObject[] adj = Grid.Instance.Objects.AdjacentObjects(GetPosition(),true);
		
		bool E = adj[0] as GridPath != null;
		bool W = adj[1] as GridPath != null;
		bool N = adj[2] as GridPath != null;
		bool S = adj[3] as GridPath != null;
		
		EdgeEast.SetActive(E);
		EdgeWest.SetActive(W);
		EdgeNorth.SetActive(N);
		EdgeSouth.SetActive(S);

		CornerNorthEast.SetActive(adj[4] as GridPath != null && N && E);
		CornerSouthWest.SetActive(adj[5] as GridPath != null && S && W);
		CornerSouthEast.SetActive(adj[6] as GridPath != null && S && E);
		CornerNorthWest.SetActive(adj[7] as GridPath != null && N && W);
	}
}
