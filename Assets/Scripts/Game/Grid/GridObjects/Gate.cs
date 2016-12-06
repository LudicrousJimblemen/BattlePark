using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BattlePark {
	public class GateData : GridObjectData {
		//
	}
	
	public class Gate : GridObject {
		#region Data Variables
		#endregion
	
		#region Serialization
		public override GridObjectData Serialize() {
			return new GateData {
				Direction = Direction,
				X = X,
				Y = Y,
				Z = Z
			};
		}
	
		public override void Deserialize(GridObjectData message) {
			GateData deserialized = (GateData)message;
		
			Direction = deserialized.Direction;
			X = deserialized.X;
			Y = deserialized.Y;
			Z = deserialized.Z;
		}
		#endregion
		
		private void OnDrawGizmos() {
			foreach (var offset in RotatedOffsets()) {
				var position = GridPosition() + offset;
				position.Scale(new Vector3(1f, 0.5f, 1f));
				Gizmos.DrawWireCube(position + new Vector3(0.5f, 0.5f, 0.5f), new Vector3(Grid.GridStepXZ, Grid.GridStepY, Grid.GridStepXZ));
			}
		}
	}
}