using System;
using UnityEngine;

public abstract class Desire {
	public GameObject Target;
}

public class DesireFood : Desire {
	public DesireFood(ItemFood food) {
		this.Food = food;
	}
	
	public ItemFood Food;
}