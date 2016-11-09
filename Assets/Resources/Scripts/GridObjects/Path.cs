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
	#endregion
	
	#region Serialization
	public override string Serialize()
	{
		return JsonConvert.SerializeObject(new PathData {
			Direction = Direction,
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