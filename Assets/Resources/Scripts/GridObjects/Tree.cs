using System;
using Newtonsoft.Json;
using UnityEngine;

public class Tree : GridObject {
	public bool SpinsALot;
	
	public override void Update() {
		base.Update();
		
		if (SpinsALot) {
			transform.Rotate(0, 0, 12);
		}
	}
	
	public override void Deserialize(string message) {
		this = JsonConvert.DeserializeObject<Tree>(message);
	}
}