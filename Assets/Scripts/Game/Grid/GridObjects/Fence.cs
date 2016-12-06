using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BattlePark {
	public class FenceData : GridObjectData {
		//
	}
	
	public class Fence : GridObject {
		#region Data Variables
		#endregion
	
		#region Serialization
		public override GridObjectData Serialize() {
			return new FenceData {
				Direction = Direction,
				X = X,
				Y = Y,
				Z = Z
			};
		}
	
		public override void Deserialize(GridObjectData message) {
			FenceData deserialized = (FenceData)message;
		
			Direction = deserialized.Direction;
			X = deserialized.X;
			Y = deserialized.Y;
			Z = deserialized.Z;
		}
		#endregion
	}
}