using System;
using Newtonsoft.Json;
using UnityEngine;

public class Sculpture : GridObject {
	public class SculptureData : GridObjectData {
		//
	}
	
	public override string Serialize() {
		return JsonConvert.SerializeObject(new SculptureData {
			Direction = Direction
		});
	}
	
	public override void Deserialize(string message) {
		SculptureData deserialized = JsonConvert.DeserializeObject<SculptureData>(message);
		
		Direction = deserialized.Direction;
	}
}