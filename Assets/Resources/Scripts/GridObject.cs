using System;
using Newtonsoft.Json;
using UnityEngine;

public class GridObject : MonoBehaviour
{
	#region Data Variables
	public class GridObjectData
	{
		public Direction Direction;
		public Vector3 Location;
	}
	
	public Direction Direction;
	public Vector3 Location;
	
	public Vector3[] OccupiedOffsets = { Vector3.zero };
	#endregion
	
	#region Serialization
	public virtual string Serialize()
	{
		return JsonConvert.SerializeObject(new GridObjectData {
			Direction = Direction,
			Location = Location
		});
	}
	
	public virtual void Deserialize(string message)
	{
		GridObjectData deserialized = JsonConvert.DeserializeObject<GridObjectData>(message);
		
		Direction = deserialized.Direction;
		Location = deserialized.Location;
	}
	#endregion
	
	public virtual void Start()	{}
	public virtual void Update() {}
	
	public virtual void OnPlaced() {}
	public virtual void OnDemolished() {}
}