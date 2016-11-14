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