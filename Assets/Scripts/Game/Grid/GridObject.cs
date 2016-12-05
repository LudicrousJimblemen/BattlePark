using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BattlePark {
	public class GridObjectData {
		public Direction Direction;
		public int X;
		public int Y;
		public int Z;
	}
	
	public abstract class GridObject : MonoBehaviour {
		#region Data Variables
		public Direction Direction;
		public int X;
		public int Y;
		public int Z;
		
		public bool PlaceMultiple;
		public Vector3[] OccupiedOffsets = { Vector3.zero };
		#endregion
	
		public long Owner;
		
		public Grid Grid;
	
		#region Serialization
		public virtual GridObjectData Serialize() {
			return new GridObjectData {
				Direction = Direction,
				X = X,
				Y = Y,
				Z = Z
			};
		}
	
		public virtual void Deserialize(GridObjectData message) {
			GridObjectData deserialized = (GridObjectData)message;
		
			Direction = deserialized.Direction;
			X = deserialized.X;
			Y = deserialized.Y;
			Z = deserialized.Z;
		}
		#endregion
	
		public virtual void OnPlaced() { }
		public virtual void OnDemolished() { }
	
		public void UpdatePosition() {
			transform.position = new Vector3 {
				x = X * Grid.GridStepXZ + 0.5f,
				y = Y * Grid.GridStepY,
				z = Z * Grid.GridStepXZ + 0.5f
			};
			transform.rotation = Quaternion.Euler(-90, 0, (int)Direction * 90);
		}
	
		public Vector3 GridPosition() {
			return new Vector3(X, Y, Z);
		}
	
		public Vector3[] RotatedOffsets() {
			Vector3[] ReturnList = new Vector3[OccupiedOffsets.Length];
			//Default is south
			//	multiply x and z by 1
			//East
			//	x becomes z, z become x
			//North
			//	multiply x and z by -1
			//West
			//	x becomes -z, z becomes -x
			switch (Direction) {
				case Direction.East:
					for (int i = 0; i < OccupiedOffsets.Length; i++) {
						ReturnList[i] = new Vector3 {
							x = -OccupiedOffsets[i].z,
							y = OccupiedOffsets[i].y,
							z = OccupiedOffsets[i].x,
						};
					}
					return ReturnList;
				case Direction.North:
					for (int i = 0; i < OccupiedOffsets.Length; i++) {
						ReturnList[i] = new Vector3 {
							x = -OccupiedOffsets[i].x,
							y = OccupiedOffsets[i].y,
							z = -OccupiedOffsets[i].z,
						};
					}
					break;
				case Direction.West:
					for (int i = 0; i < OccupiedOffsets.Length; i++) {
						ReturnList[i] = new Vector3 {
							x = OccupiedOffsets[i].z,
							y = OccupiedOffsets[i].y,
							z = -OccupiedOffsets[i].x,
						};
					}
					break;
				case Direction.South:
					ReturnList = OccupiedOffsets;
					break;
				default:
					return null;
			}
			return ReturnList;
		}
	}
}