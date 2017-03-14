using UnityEngine;

namespace Pathfinding {
	public struct PathConnection {
		public PathNode n1, n2;
		public int ExternalCost;
		public readonly float distance;
		public PathConnection (PathNode n1, PathNode n2, int cost = 0) {
			this.n1 = n1;
			this.n2 = n2;
			distance = Vector3.Distance(n1.Position, n2.Position);
			ExternalCost = cost;
		}
	}
}