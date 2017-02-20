using UnityEngine;
using System.Linq;

public class GridPath : GridObject {
	public override Vector3[] OccupiedOffsets {
		get {
			return new[] { Vector3.zero };
		}
	}

	public override bool PlaceMultiple { get { return true; } }
	public override bool CanRotate { get { return false; } }

	public override bool Valid(Vector3 position,Direction direction,int player) {
		//Debug.Log(Grid.Instance.Objects.AdjacentObjects(position).Count);
		return base.Valid(position,direction,player) && Grid.Instance.Objects.AdjacentObjects (position).Any (x => x.GetType () == typeof(GridPath));
	}
}
