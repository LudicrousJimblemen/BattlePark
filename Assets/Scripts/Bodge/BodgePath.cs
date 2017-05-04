using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class BodgePath : MonoBehaviour {
	private PathNode[] Nodes;
	public NodeGraph Graph;

	public void Start() {
		Nodes = new PathNode[3 * 3];
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				Vector3 nodePos = BodgeGrid.Instance.SnapToGrid(transform.position) + new Vector3(-1f, 0, -1f) + new Vector3(i, 0, j);
				Nodes[i * 3 + j] =  Graph.AddNode(nodePos, Graph.ScanDistance);
			}
		}
	}
}
