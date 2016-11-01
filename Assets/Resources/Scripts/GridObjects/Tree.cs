using System;
using Newtonsoft.Json;
using UnityEngine;

public class Tree : GridObject {
	public class TreeData : GridObjectData {
		public bool SpinsALot;
	}
	
	public bool SpinsALot;
	
	public override void Update() {
		base.Update();
		
		if (SpinsALot) { //TEST SECTION DELETE NOW
			GetComponent<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 1f, 1f, 1f);
		}
	}
	
	public override string Serialize() {
		return JsonConvert.SerializeObject(new TreeData {
			Direction = Direction,
			
			SpinsALot = SpinsALot
		});
	}
	
	public override void Deserialize(string message) {
		TreeData deserialized = JsonConvert.DeserializeObject<TreeData>(message);
		
		Direction = deserialized.Direction;
		
		SpinsALot = deserialized.SpinsALot;
	}
}