using UnityEngine;

public class GridSculpture : GridObject {
	public override bool PlaceMultiple { get { return false; } }
	public override Vector3[] OccupiedOffsets {	get { return new[] {
		Vector3.zero,
		new Vector3 (0,0,1),
		new Vector3 (0,0,2),
		new Vector3 (0,0,3),
		new Vector3 (1,0,0),
		new Vector3 (2,0,0),
		new Vector3 (3,0,0)
	};}}
}
