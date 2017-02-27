using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class GridVendor : GridObject {
	public abstract VendorItem AllowedProductFlags { get; }
	public abstract VendorItem ProductFlags { get; set; }
	public abstract List<VendorProduct> Products { get; set; }
}