using System;
using UnityEngine;

public class GridSculpture : GridObject {
	public override int Cost { get { return 20000; } }

	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				Vector3.zero,
				new Vector3(0, 1, 0),
				new Vector3(0, 2, 0)
			};
		}
	}
}
