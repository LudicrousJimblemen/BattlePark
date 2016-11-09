using System;
using Newtonsoft.Json;
using UnityEngine;

public class GridObject : MonoBehaviour
{
	#region Data Variables
	public class GridObjectData
	{
		public Direction Direction;
	}
	
	public Direction Direction;
	#endregion
	
	#region Serialization
	public virtual string Serialize()
	{
		return JsonConvert.SerializeObject(new GridObjectData {
			Direction = Direction
		});
	}
	
	public virtual void Deserialize(string message)
	{
		GridObjectData deserialized = JsonConvert.DeserializeObject<GridObjectData>(message);
		Direction = deserialized.Direction;
	}
	#endregion
	
	public virtual void Start()
	{
		Serialize();
		
		transform.rotation = Quaternion.Euler(-90, 0, (int)(Direction) * 90);
	}
	
	public virtual void Update();
	
	public virtual void OnPlaced();
	public virtual void OnDemolished();
}