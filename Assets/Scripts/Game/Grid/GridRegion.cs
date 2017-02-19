using UnityEngine;

public struct GridRegion {
	public float X { get; set; }
	public float Z { get; set; }
	public float Width { get; set; }
	public float Length { get; set; }

	public long Owner;

	public GridRegion(float x, float z, float width, float length, long owner) {
		this.X = x;
		this.Z = z;
		this.Width = width;
		this.Length = length;
		this.Owner = owner;
	}
	public GridRegion(Vector2 v1,Vector2 v2,long owner) {
		this.X = v1.x;
		this.Z = v1.y;
		this.Width = Mathf.Abs(v2.x-v1.x);
		this.Length = Mathf.Abs(v2.y - v1.y);
		this.Owner = owner;
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
	public bool Inside (Vector3 position, Vector3[] offsets) {
		if(!Inside(position)) {
			//Debug.Log("origin not within valid region");
			return false;
		}
		for(int i = 0; i < offsets.Length; i++) {
			if(!Inside(offsets[i] + position)) {
				//Debug.Log(string.Format("index {0} not within valid region",i.ToString()));
				return false;
			}
		}
		return true;
	}

	public Vector3 GetCenter(Grid grid) {
		return new Vector3 {
			x = X + Width / 2f,
			y = 0,
			z = Z + Length / 2f
		};
	}
}
