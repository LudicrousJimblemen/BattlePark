using System;
using Newtonsoft.Json;
using UnityEngine;

public class GridObject : MonoBehaviour {
	public bool Active;
	
	public Direction Direction;
	
	public virtual void Start() {
		Serialize();
		
		transform.Rotate(-90, 0, (int)(Direction) * 90);
	}
	
	public virtual void Update() {
		Active = !GetComponent<GridPlaceholder>();
	}
	
	public virtual string Serialize() {
		return JsonConvert.SerializeObject(this);
	}
	
	public virtual void Deserialize(string message) {
		this = JsonConvert.DeserializeObject<GridObject>(message);
	}
}