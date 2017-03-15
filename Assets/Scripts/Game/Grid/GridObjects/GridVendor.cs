using System;
using UnityEngine;

public abstract class GridVendor : GridObject {
	/// <summary>
	/// The product being sold by a vendor.
	/// </summary>
	public abstract Item Product { get; }
	
	/// <summary>
	/// Tries to sell a vendor's product to a person.
	/// </summary>
	/// <param name="person">The person to whom the product is being sold.</param>
	/// <returns>True if the transaction was successful, false otherwise.</returns>
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
	
	protected void Awake() {
		Product.Price = Product.DefaultPrice;
	}
}
