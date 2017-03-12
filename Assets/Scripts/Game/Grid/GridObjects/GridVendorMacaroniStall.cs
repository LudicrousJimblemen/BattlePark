using System;
using UnityEngine;

public class GridVendorMacaroniStall : GridVendor {
	public override Money Cost { get { return new Money(200, 00); } }

	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				Vector3.zero,
				new Vector3(0, 1, 0)
			};
		}
	}

	public override Item Product { get { return ItemFood.Macaroni; } }
}
