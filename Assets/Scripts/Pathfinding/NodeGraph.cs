using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Pathfinding {
	public class NodeGraph : MonoBehaviour {
		
		[HideInInspector]
		public enum logLevel {
			None,
			Debug
		}
		public logLevel LogLevel = logLevel.None;
		
		[Tooltip("The maximum distance that new nodes will scan and create connections with other nodes")]
		public float ScanDistance = 5f;
		
		[HideInInspector]
		public Dictionary<Vector3, PathNode> Nodes = new Dictionary<Vector3, PathNode>();
	
		private Queue<PathResult> results = new Queue<PathResult>();
		private List<PathRequest> requests = new List<PathRequest>();
		public PathNode AddNode (Vector3 position, bool scan = true) {
			if(Nodes.ContainsKey(position))
				return null;
			PathNode node = new PathNode(this,position,scan);
			Nodes.Add (position, node);
			return node;
		}
		public void RemoveNode (PathNode node) {
			if (!Nodes.ContainsKey(node.Position))
				return;
			foreach (PathConnection conn in node.Connections) {
				node.RemoveConnection (conn);
			}
			Nodes.Remove (node.Position);
		}
		public PathNode ClosestNode (Vector3 position) {
			if(Nodes.Count == 0)
				return null;
			return Nodes.OrderBy(x => Vector3.SqrMagnitude(position - x.Key)).First().Value;
		}
		/// <summary>
		/// Calculates a path from a start position to a destination, visiting existing nodes based on their connections to each other
		/// Runs a specified callback with returned path and success
		/// </summary>
		/// <param name="start">The position to start at, need not be the position of an existing node</param>
		/// <param name="destination">The position to end up at, need not be the position of an exisiting node</param>
		/// <param name="callback">The method to be called upon receiving the path</param>
		/// <returns>Returns a Path object, simply a list of destinations to visit in order. If a path is not found, returns null</returns>
		public void RequestPath (Transform start, Vector3 destination, Action<Path, bool> callback, int priority = 0) {
			PathRequest request = new PathRequest(start,destination,callback,priority);
			requests.Add(request);
		}
		private void Update () {
			if (requests.Count > 0) {
				PathRequest request = requests
					.OrderByDescending(x => x.priority) // order requests by highest priority
					.ThenBy(x => x.time).First(); // then, order them by earliest
				requests.Remove(request);
				lock (requests) {
					ThreadStart thread = delegate {
						CalculatePath(request,OnReceivedPath);
					};
					thread.Invoke();
				}
			}
			if (results.Count > 0) {
				lock (results) {
					for (int i = 0; i < results.Count; i++) {
						PathResult result = results.Dequeue ();
						result.callback (result.path, result.success);
					}
				}
			}
		}
		private void CalculatePath(PathRequest request, Action<PathResult> callback) {
			PathNode startNode = ClosestNode(request.start.position); //Node startNode = grid.NodeFromWorldPoint(startPos);
			PathNode destNode = ClosestNode(request.destination); //Node destNode = grid.NodeFromWorldPoint(targetPos);
			List<PathNode> open = new List<PathNode>();
			HashSet<PathNode> closed = new HashSet<PathNode>();
			open.Add(startNode);
			
			bool success = false;
			
			Stopwatch sw = new Stopwatch ();
			if (LogLevel == logLevel.Debug) {
				sw.Start ();
			}
			
			while(open.Count > 0) {			
				PathNode node = open[0];
				for(int i = 1; i < open.Count; i++) {
					if(open[i].F < node.F || Mathf.Abs(open[i].F - node.F) < 0.01f) {
						if(open[i].F < node.F)
							node = open[i];
					}
				}
	
				open.Remove(node);
				closed.Add(node);
	
				if(node == destNode) {
					if (LogLevel == logLevel.Debug) {
						sw.Stop ();
					}
					success = true;
					break;
				}
	
				foreach(PathConnection conn in node.Connections) {
					PathNode neighbour = conn.n2;
					if(closed.Contains(neighbour)) {
						continue;
					}
	
					float newCostToNeighbour = node.G + conn.distance;
					if(newCostToNeighbour < neighbour.G || !open.Contains(neighbour)) {
						neighbour.G = newCostToNeighbour;
						neighbour.H = Vector3.Distance(request.destination,node.Position);
						neighbour.Parent = node;
	
						if(!open.Contains(neighbour))
							open.Add(neighbour);
					}
				}
			}
			Path foundPath = null;
			if (success) { // it found the end node
				foundPath = RetracePath(startNode,destNode);
				if(LogLevel == logLevel.Debug) {
					print(string.Format("Path found: {0} ms",sw.ElapsedMilliseconds));
				}
			}
			callback (new PathResult (foundPath, success, request, request.callback));
		}
		private Path RetracePath(PathNode startNode,PathNode endNode) {
			List<PathNode> path = new List<PathNode>();
			PathNode currentNode = endNode;
	
			while(currentNode != startNode) {
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}
			path.Add(startNode);
	        path.Reverse();
			return new Path(path);
		}
		private void OnReceivedPath (PathResult result) {
			lock (results) {
				results.Enqueue (result);
			}
		}
		void OnDrawGizmos () {
			if (Nodes != null) {
				foreach (KeyValuePair<Vector3, PathNode> node in Nodes) {
					Gizmos.color = Color.black;
					Gizmos.DrawSphere(node.Key,0.2f);
					Gizmos.color = Color.green;
					foreach (PathConnection conn in node.Value.Connections) {
						Gizmos.DrawLine(conn.n1.Position,conn.n2.Position);
					}
				}
			}
		}
	}
	
	public struct PathRequest {
		public Transform start;
		public Vector3 destination;
		public Action<Path, bool> callback;
		public int priority;
		public float time;
		public PathRequest (Transform _start, Vector3 _destination, Action<Path, bool> _callback, int _priority = 0) {
			start = _start;
			destination = _destination;
			callback = _callback;
			priority = _priority;
			time = Time.time;
		}
	}
	public struct PathResult {
		public Path path;
		public bool success;
		public Action<Path, bool> callback;
		public PathRequest request;
		public PathResult (Path _path, bool _success, PathRequest _request, Action<Path, bool> _callback) {
			path = _path;
			success = _success;
			request = _request;
			callback = _callback;
		}
	}
}