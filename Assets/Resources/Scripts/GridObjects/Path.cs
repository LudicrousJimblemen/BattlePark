using System;
using Newtonsoft.Json;
using UnityEngine;

public class Path : GridObject {
	#region Data Variables
	public class PathData : GridObjectData {
		//
	}
	
	#endregion
	
	#region Serialization
	public override string Serialize() {
		return JsonConvert.SerializeObject(new PathData {
			Direction = Direction,
			X = X,
			Y = Y,
			Z = Z
		});
	}
	
	public override void Deserialize(string message) {
		PathData deserialized = JsonConvert.DeserializeObject<PathData>(message);
		
		Direction = deserialized.Direction;
		X = deserialized.X;
		Y = deserialized.Y;
		Z = deserialized.Z;
	}
	#endregion
	
	public override void Start() {
		base.Start();
		
		OccupiedOffsets = new [] { 
			new Vector3(0, 1, 0)
		};
	}
	
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