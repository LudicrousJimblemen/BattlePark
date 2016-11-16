using System;
using Newtonsoft.Json;
using UnityEngine;

public class Tree : GridObject
{
	#region Data Variables
	public class TreeData : GridObjectData
	{
		public bool SpinsALot;
	}
	
	public bool SpinsALot;
	public Vector3[] OccupiedOffsets = new Vector3[5] { Vector3.zero,new Vector3 (0,1,0),new Vector3 (0,2,0),new Vector3 (0,3,0),new Vector3 (0,4,0) };
	#endregion

	#region Serialization
	public override string Serialize()
	{
		return JsonConvert.SerializeObject(new TreeData {
			Direction = Direction,
			X = X,
			Y = Y,
			Z = Z,
			
			SpinsALot = SpinsALot
		});
	}
	
	public override void Deserialize(string message)
	{
		TreeData deserialized = JsonConvert.DeserializeObject<TreeData>(message);
		
		Direction = deserialized.Direction;
		X = deserialized.X;
		Y = deserialized.Y;
		Z = deserialized.Z;
		
		SpinsALot = deserialized.SpinsALot;
	}
	#endregion
}