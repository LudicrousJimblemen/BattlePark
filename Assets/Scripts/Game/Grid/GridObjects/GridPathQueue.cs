using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridPathQueue : GridPath {
	protected override string languageId { get { return "pathQueue"; } }
	
	public GridObject[] Connections = new GridObject[4];
	
	public override void UpdateMesh() {
		GridObject[] adj = Grid.Instance.Objects.AdjacentObjects(GetPosition(), true);
		
		for (int i = 0; i < 4; i++) {
			if (!(adj[i] is GridPath)) {
				Connections[i] = null;
			}
		}
		
		for (int i = 0; i < 4; i++) {
			if (Connections.Count(c => c != null) >= 2) {
				break;
			}
			
			if (adj[i] is GridPath) {
				GridPathQueue adjacentQueue = adj[i] as GridPathQueue;
				if (adjacentQueue != null) {
					if (adjacentQueue.Connections.Count(c => c != null) < 2 || adjacentQueue.Connections.Contains(this)) {
						Connections[i] = adj[i];
					}
				} else {
					Connections[i] = adj[i];
				}
			}
		}

		EdgeWest.SetActive(Connections[0] is GridPath);
		EdgeEast.SetActive(Connections[1] is GridPath);
		EdgeSouth.SetActive(Connections[2] is GridPath);
		EdgeNorth.SetActive(Connections[3] is GridPath);

		CornerSouthWest.SetActive(false);
		CornerNorthWest.SetActive(false);
		CornerSouthEast.SetActive(false);
		CornerNorthEast.SetActive(false);
	}
}
