using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridVendor : GridVendor {
	public override List<VendorProduct> Products { get; set; }
	
	private void Start() {
		Products = new List<VendorProduct> {
			new VendorProduct { Cost = 150, Explosivosity = 0, Item = new VendorMacaroni() }
		};
	}
}