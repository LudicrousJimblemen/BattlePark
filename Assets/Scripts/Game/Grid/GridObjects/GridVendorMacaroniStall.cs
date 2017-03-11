using System;
using UnityEngine;

public abstract class GridVendorNacaromiStall : GridObject {
	public override Money Cost { get { return new Money(200, 00); } }

	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				Vector3.zero,
				new Vector3(0, 1, 0)
			};
		}
	}
	
	public Item Product = ItemFood.Macaroni;
}
