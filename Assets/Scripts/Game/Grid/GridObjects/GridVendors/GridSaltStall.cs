using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridSaltStall : GridVendor {
	public override VendorItem AllowedProductFlags { get { return VendorItem.SaltLick; } }
	
	public override VendorItem ProductFlags { get; set; }
	public override List<VendorProduct> Products { get; set; }
	
	public override int Cost { get { return 350000; } }
	
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
		
		ProductFlags = VendorItem.SaltLick;
		Products = new List<VendorProduct> {
			new VendorProduct { Cost = 75, Item = VendorItem.SaltLick }
		};
	}
}