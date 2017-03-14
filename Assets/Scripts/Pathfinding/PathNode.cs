using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding {
	public class PathNode {
		public Vector3 Position;
		// a list of all connections to other nodes
		public List<PathConnection> Connections = new List<PathConnection>();
		private NodeGraph graph;
	
		public float G;
		public float H;
		public float F {
			get {
				return G + H;
			}
		}
	
		public PathNode Parent;
	
		public PathNode (NodeGraph graph, Vector3 position, bool scan = true) {
			Position = position;
			this.graph = graph;
			if (scan) {
				foreach(KeyValuePair<Vector3,PathNode> entry in this.graph.Nodes.Where(x => Vector3.SqrMagnitude(Position - x.Key) <= this.graph.ScanDistance * this.graph.ScanDistance)) {
					AddConnection(entry.Value);
				}
			}
		}
		public void AddConnection (PathNode node) {
			Connections.Add (new PathConnection(this, node));
			node.Connections.Add(new PathConnection(node,this));
		}
		public void RemoveConnection (PathConnection connection) {
			connection.n2.Connections.Remove (connection);
			Connections.Remove (connection);
		}
		public void RemoveConnection (PathNode node) {
			Connections.Remove (Connections.Where (x => x.n2 == node).First());
			node.Connections.Remove (node.Connections.Where (x => x.n2 == this).First());
		}
	}
}