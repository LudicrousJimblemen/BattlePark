using System;
using Newtonsoft.Json;
using UnityEngine;

public class Path : GridObject
{
	#region Data Variables
	public class PathData : GridObjectData
	{
		public bool OccupiedNorth;
		public bool OccupiedEast;
		public bool OccupiedSouth;
		public bool OccupiedWest;
	}
	
	public bool OccupiedNorth;
	public bool OccupiedEast;
	public bool OccupiedSouth;
	public bool OccupiedWest;
	public Vector3[] OccupiedOffsets = new Vector3[1] { Vector3.zero };
	#endregion
	
	#region Serialization
	public override string Serialize()
	{
		return JsonConvert.SerializeObject(new PathData {
			Direction = Direction,
			X = X,
			Y = Y,
			Z = Z,
				
			OccupiedNorth = OccupiedNorth,
			OccupiedEast = OccupiedEast,
			OccupiedSouth = OccupiedSouth,
			OccupiedWest = OccupiedWest
		});
	}
	
	public override void Deserialize(string message)
	{
		PathData deserialized = JsonConvert.DeserializeObject<PathData>(message);
		
		Direction = deserialized.Direction;
		X = deserialized.X;
		Y = deserialized.Y;
		Z = deserialized.Z;
		
		OccupiedNorth = deserialized.OccupiedNorth;
		OccupiedEast = deserialized.OccupiedEast;
		OccupiedSouth = deserialized.OccupiedSouth;
		OccupiedWest = deserialized.OccupiedWest;
	}
	#endregion
	
	public override void OnPlaced()
	{
		//update own occupation variables
		//foreach surrounding path
			//update occupation variables
			//update mesh
		//update own mesh
	}
}