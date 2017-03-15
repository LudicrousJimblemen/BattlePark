using System;
using UnityEngine;

public abstract class Desire {
	public GameObject Target;
	
	public static explicit operator DesireType(Desire desire) {
		if (desire is DesireFood) {
			return DesireType.Food;
		}
		if (desire is DesireAttraction) {
			return DesireType.Attraction;
		}
		throw new InvalidCastException("Invalid cast: No relevant DesireType");
	}
}

public enum DesireType {
	Food,
	Attraction
}

public class DesireFood : Desire {
	public DesireFood(ItemFood food) {
		this.Food = food;
	}
	
	public ItemFood Food;
}

public class DesireAttraction : Desire {
	public DesireAttraction(Attraction attraction) {
		this.Attraction = attraction;
	}
	
	public Attraction Attraction;
}