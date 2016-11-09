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
			
			SpinsALot = SpinsALot
		});
	}
	
	public override void Deserialize(string message)
	{
		TreeData deserialized = JsonConvert.DeserializeObject<TreeData>(message);
		
		Direction = deserialized.Direction;
		
		SpinsALot = deserialized.SpinsALot;
	}
	#endregion
}