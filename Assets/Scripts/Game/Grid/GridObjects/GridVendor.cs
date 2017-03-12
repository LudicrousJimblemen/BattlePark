using System;
using UnityEngine;

public abstract class GridVendor : GridObject {
	public override Money Cost { get { return new Money(200, 00); } }

	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				Vector3.zero,
				new Vector3(0, 1, 0)
			};
		}
	}
	
	public abstract Item Product { get; }
	
	public bool SellTo(Person person) {
		if (person.Money >= Product.Price) {
			person.Money -= Product.Price;
			
			ItemFood foodProduct = Product as ItemFood;
			if (foodProduct != null) {
				person.Inventory.Add(new InventoryFood { Food = foodProduct, Amount = foodProduct.ServingSize });
			}

			return true;
		}

		return false;
	}
}
