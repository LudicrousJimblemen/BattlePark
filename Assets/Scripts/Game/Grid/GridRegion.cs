using UnityEngine;
using System.Collections;

public struct GridRegion {
	public int X;
	public int Z;
	public int Width;
	public int Length;

	public long Owner;

	public GridRegion(int x, int z, int width, int length, long owner) {
		this.X = x;
		this.Z = z;
		this.Width = width;
		this.Length = length;
		this.Owner = owner;
	}

	public void SetOwner(long playerId) {
		Owner = playerId;
	}

	public bool Valid(long id) {
		switch (Owner) {
			case -1:
				return false;
			case 0: 
				return true;
			default:
				return id == Owner;
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
