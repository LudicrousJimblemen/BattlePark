using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public abstract class GridPath : GridObject {
	public GameObject EdgeSouth;
	public GameObject EdgeNorth;
	public GameObject EdgeWest;
	public GameObject EdgeEast;
	public GameObject CornerSouthWest;
	public GameObject CornerSouthEast;
	public GameObject CornerNorthWest;
	public GameObject CornerNorthEast;
	
	private PathNode[] Nodes;

	public override Money Cost { get { return new Money(10, 00); } }

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
		
		ServerAddNodes (3);
	}

	public override bool CanRotate { get { return false; } }

	public virtual void UpdateMesh() {
		GridObject[] adj = Grid.Instance.Objects.AdjacentObjects(GetPosition(), true);
		
		bool W = adj[0] is GridPath;
		bool E = adj[1] is GridPath;
		bool S = adj[2] is GridPath;
		bool N = adj[3] is GridPath;

		EdgeWest.SetActive(W);
		EdgeEast.SetActive(E);
		EdgeSouth.SetActive(S);
		EdgeNorth.SetActive(N);

		CornerSouthWest.SetActive(adj[4] is GridPath && S && W);
		CornerNorthWest.SetActive(adj[5] is GridPath && N && W);
		CornerSouthEast.SetActive(adj[6] is GridPath && S && E);
		CornerNorthEast.SetActive(adj[7] is GridPath && N && E);
	}
	
	[Server]
	private void ServerAddNodes (int width) {
		NodeGraph graph = GameManager.Instance.Graphs[Owner-1];
		Nodes = new PathNode[width * width];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < width; j++) {
				Vector3 nodePos = GetPosition() + new Vector3(-1f, 0, -1f) + new Vector3(i, 0, j);
				Nodes[i * width + j] =  graph.AddNode(nodePos, graph.ScanDistance);
			}
		}
	}
}
