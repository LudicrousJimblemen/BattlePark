using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BattlePark {
	public class PathData : GridObjectData {
		//
	}
	
	public class Path : GridObject {
		#region Data Variables
		#endregion
	
		#region Serialization
		public override GridObjectData Serialize() {
			return new PathData {
				Direction = Direction,
				X = X,
				Y = Y,
				Z = Z
			};
		}
	
		public override void Deserialize(GridObjectData message) {
			PathData deserialized = (PathData)message;
		
			Direction = deserialized.Direction;
			X = deserialized.X;
			Y = deserialized.Y;
			Z = deserialized.Z;
		}
		#endregion
	
		public override void OnPlaced() {
			foreach (var gridObject in Grid.Objects.AdjacentObjects(GridPosition(), true)) {
				if (gridObject.GetComponent<Path>() != null) {
					gridObject.GetComponent<Path>().UpdatePath();
					UpdatePath();
				}
			}
		}
	
		public void UpdatePath() {
			//TODO generate mesh
		}
	}
}