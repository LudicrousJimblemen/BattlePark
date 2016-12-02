using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BattlePark {
	public class SculptureData : GridObjectData {
		//
	}
	
	public class Sculpture : GridObject {
		#region Data Variables
		//
		#endregion
		
		#region Serialization
		public override GridObjectData Serialize() {
			return new SculptureData {
				Direction = Direction,
				X = X,
				Y = Y,
				Z = Z
			};
		}
	
		public override void Deserialize(GridObjectData message) {
			SculptureData deserialized = (SculptureData)message;
		
			Direction = deserialized.Direction;
			X = deserialized.X;
			Y = deserialized.Y;
			Z = deserialized.Z;
		}
		#endregion
	}
}