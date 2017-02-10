using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSculpture : GridObject {
	public override bool PlaceMultiple { get { return false; } }
	public override Vector3[] OccupiedOffsets {	get { return new [] 
			{ Vector3.zero }; 
		} }
}
