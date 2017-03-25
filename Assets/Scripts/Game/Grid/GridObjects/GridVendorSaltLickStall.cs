using System;
using UnityEngine;

public class GridVendorSaltLickStall : GridVendor {
	protected override string languageId { get { return "vendorSaltLickStall"; } }
	
	public override Money Cost { get { return new Money(175, 00); } }
	
	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				Vector3.zero,
				new Vector3(0, 1, 0)
			};
		}
	}

	public override Item Product { get { return ItemFood.SaltLick; } }
}
