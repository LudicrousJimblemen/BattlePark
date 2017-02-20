using UnityEngine;

public class GridPath : GridObject {
	public override Vector3[] OccupiedOffsets {
		get {
			return new[] { Vector3.zero };
		}
	}

	public override bool PlaceMultiple { get { return true; } }
	public override bool CanRotate { get { return false; } }
}
