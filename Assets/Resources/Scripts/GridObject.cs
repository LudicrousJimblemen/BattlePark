using System;
using Newtonsoft.Json;
using UnityEngine;

public class GridObject : MonoBehaviour
{
	public class GridObjectData
	{
		public Direction Direction;
	}
	
	public bool Active;
	
	public Direction Direction;
	
	public virtual void Start()
	{
		Serialize();
		
		transform.rotation = Quaternion.Euler(-90, 0, (int)(Direction) * 90);
	}
	
	public virtual void Update()
	{
		Active = !GetComponent<GridPlaceholder>();
	}
	
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
}