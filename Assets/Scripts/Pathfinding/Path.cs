using System.Collections;
using System.Collections.Generic;

namespace Pathfinding {
	public class Path : IEnumerable {
		// a list of all nodes to visit, in order of index
		private List<PathNode> Nodes;
		public IEnumerator GetEnumerator () {
			return Nodes.GetEnumerator();
		}
		public int Count {
			get {
				return Nodes.Count;
			}
		}
		public Path (List<PathNode> nodes) {
			Nodes = nodes;
		}
		
		public PathNode this[int i] {
			get {
				return Nodes[i];
			}
		}
	}
}