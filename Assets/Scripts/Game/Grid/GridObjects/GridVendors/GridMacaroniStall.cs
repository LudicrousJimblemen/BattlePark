using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridMacaroniStall : GridVendor {
	public override List<VendorProduct> Products { get; set; }
	
	public override int Cost { get { return 400000; } }
	
	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				Vector3.zero,
				new Vector3(0, 1, 0)
			};
		}
	}
	
	public override void OnPlaced() {
		base.OnPlaced();
		
		Products = new List<VendorProduct> {
			new VendorProduct { Cost = 150, Item = new VendorMacaroni() }
		};
	}
}