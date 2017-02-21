using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class GridVendor : GridObject {
	public abstract List<VendorProduct> Products { get; set; }
}