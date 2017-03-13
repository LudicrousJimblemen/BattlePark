﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class GridPath : GridObject {
	public GameObject EdgeSouth;
	public GameObject EdgeNorth;
	public GameObject EdgeWest;
	public GameObject EdgeEast;
	public GameObject CornerSouthWest;
	public GameObject CornerSouthEast;
	public GameObject CornerNorthWest;
	public GameObject CornerNorthEast;

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
		
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				Vector3 nodePos = GetPosition() + new Vector3(-.75f, 0, -.75f) + new Vector3(1.5f*i, 0, 1.5f*j);
				Instantiate(GameManager.Instance.PathNode, nodePos, Quaternion.identity, GameObject.FindGameObjectWithTag("PathNode").transform);
			}
		}
		AstarPath.active.Scan ();
	}

	public override bool CanRotate { get { return false; } }

	public void UpdateMesh() {
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
}
