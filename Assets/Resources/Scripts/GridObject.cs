using System;
using Newtonsoft.Json;
using UnityEngine;

public class GridObject : MonoBehaviour {
	#region Data Variables
	public class GridObjectData {
		public Direction Direction;
		public int X;
		public int Y;
		public int Z;
	}
	
	public Direction Direction;
	public int X;
	public int Y;
	public int Z;
	
	public Vector3[] OccupiedOffsets = { Vector3.zero };
	#endregion
	
	public Grid Grid;
	
	#region Serialization
	public virtual string Serialize() {
		return JsonConvert.SerializeObject(new GridObjectData {
			Direction = Direction,
			X = X,
			Y = Y,
			Z = Z
		});
	}
	
	public virtual void Deserialize(string message) {
		GridObjectData deserialized = JsonConvert.DeserializeObject<GridObjectData>(message);
		
		Direction = deserialized.Direction;
		X = deserialized.X;
		Y = deserialized.Y;
		Z = deserialized.Z;
	}
	#endregion
	
	public virtual void Start() {
		Grid = FindObjectOfType<Grid>();
	}
	public virtual void Update() {}
	
	public virtual void OnPlaced() {}
	public virtual void OnDemolished() {}
	
	public void UpdatePosition() {
		transform.position = new Vector3 {
			x = X * Grid.GridStepXZ,
			y = Y * Grid.GridStepY,
			z = Z * Grid.GridStepXZ
		};
		transform.rotation = Quaternion.Euler(-90, 0, (int)Direction * 90);
	}
	
	public Vector3 GridPosition() {
		return new Vector3(X, Y, Z);
	}
}