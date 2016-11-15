using System;
using Newtonsoft.Json;
using UnityEngine;

public class GridObject : MonoBehaviour
{
	#region Data Variables
	public class GridObjectData
	{
		public Direction Direction;
		public int X;
		public int Y;
		public int Z;
	}
	
	public Direction Direction;
	public int X;
	public int Y;
	public int Z;
	#endregion
	
	#region Serialization
	public virtual string Serialize()
	{
		return JsonConvert.SerializeObject(new GridObjectData {
			Direction = Direction,
			X = X,
			Y = Y,
			Z = Z
		});
	}
	
	public virtual void Deserialize(string message)
	{
		GridObjectData deserialized = JsonConvert.DeserializeObject<GridObjectData>(message);
		
		Direction = deserialized.Direction;
		X = deserialized.X;
		Y = deserialized.Y;
		Z = deserialized.Z;
	}
	#endregion
	
	public virtual void Start()	{}
	public virtual void Update() {}
	
	public virtual void OnPlaced() {}
	public virtual void OnDemolished() {}
}