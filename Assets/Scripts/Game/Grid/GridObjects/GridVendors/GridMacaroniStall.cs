using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridMacaroniStall : GridVendor {
	public override List<VendorProduct> Products { get; set; }
	
	public override int Cost { get { return 400000; } }
	
	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				new Vector3(-1, 0, -1),
				new Vector3(1, 0, -1),
				new Vector3(1, 0, -1),
				new Vector3(-1, 0, 0),
				new Vector3(0, 0, 0),
				new Vector3(1, 0, 0),
				new Vector3(-1, 0, 1),
				new Vector3(0, 0, 1),
				new Vector3(1, 0, 1),
				new Vector3(-1, 1, -1),
				new Vector3(1, 1, -1),
				new Vector3(1, 1, -1),
				new Vector3(-1, 1, 0),
				new Vector3(0, 1, 0),
				new Vector3(1, 1, 0),
				new Vector3(-1, 1, 1),
				new Vector3(0, 1, 1),
				new Vector3(1, 1, 1),
				new Vector3(-1, 2, -1),
				new Vector3(1, 2, -1),
				new Vector3(1, 2, -1),
				new Vector3(-1, 2, 0),
				new Vector3(0, 2, 0),
				new Vector3(1, 2, 0),
				new Vector3(-1, 2, 1),
				new Vector3(0, 2, 1),
				new Vector3(1, 2, 1)
			};
			// TODO add a hitbox utility to generate things like this
			// this is just a 3x3 cube
		}
	}
	
	private void Start() {
		Products = new List<VendorProduct> {
			new VendorProduct { Cost = 150, Explosivosity = 0, Item = new VendorMacaroni() }
		};
	}
}