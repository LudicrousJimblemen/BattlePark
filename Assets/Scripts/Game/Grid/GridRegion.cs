using System;
using UnityEngine;

namespace BattlePark {
	public struct GridRegion {
		public int X;
		public int Z;
		public int Width;
		public int Length;
	
		public int Owner;
	
		public GridRegion(int x, int z, int width, int length, int owner) {
			this.X = x;
			this.Z = z;
			this.Width = width;
			this.Length = length;
			this.Owner = owner;
		}
	
		public void SetOwner(int playerId) {
			Owner = playerId;
		}
	
		public bool Valid(int id) {
			switch (Owner) {
				case -1:
					return false;
				case 0: 
					return true;
				default:
					Debug.Log(id + " " + Owner);
					if (id == Owner) {
						return true;
					} else {
						return false;
					}
			}
		}
	
		public bool Inside(Vector3 position) {
			return position.x >= X && position.z >= Z && position.x < X + Width && position.z < Z + Length;
		}
	
		public Vector3 GetCenter(Grid grid) {
			return new Vector3 {
				x = (X * grid.GridStepXZ) + Width / 2f,
				y = 0,
				z = (Z * grid.GridStepXZ) + Length / 2f
			};
		}
	}
}