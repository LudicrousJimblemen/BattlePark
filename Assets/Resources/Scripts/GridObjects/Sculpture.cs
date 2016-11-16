using System;
using Newtonsoft.Json;
using UnityEngine;

public class Sculpture : GridObject
{
	#region Data Variables
	public class SculptureData : GridObjectData
	{
		//
	}
	
	public Vector3[] OccupiedOffsets = { Vector3.zero, new Vector3 (0, 1, 0),new Vector3 (0,2,0) };
	#endregion

	#region Serialization
	public override string Serialize()
	{
		return JsonConvert.SerializeObject(new SculptureData {
			Direction = Direction,
			X = X,
			Y = Y,
			Z = Z
		});
	}
	
	public override void Deserialize(string message)
	{
		SculptureData deserialized = JsonConvert.DeserializeObject<SculptureData>(message);
		
		Direction = deserialized.Direction;
		X = deserialized.X;
		Y = deserialized.Y;
		Z = deserialized.Z;
	}
	#endregion
}