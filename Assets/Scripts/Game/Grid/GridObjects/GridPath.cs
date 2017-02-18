using UnityEngine;

public class GridPath : GridObject {
	
	// set this to whatever
	public override bool PlaceMultiple { get { return 
		true; 
	}}
	
	// set this to whatever
	public override Vector3[] OccupiedOffsets {	get { return new[] {
		Vector3.zero 
	};}}
}
