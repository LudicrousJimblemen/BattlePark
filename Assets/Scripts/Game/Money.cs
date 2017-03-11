﻿using System;

public struct Money {
	public int Large { get; set; }
	
	private int small;
	public int Small {
		get { return small; }
		set {
			if (value >= 100) {
				Large += value / 100;
				small = value % 100;
			}
		}
	}
	
	public Money(int large, int small) {
		this.Large = large;
		this.Small = small;
	}
		
	public override bool Equals(object obj) {
		if (obj == null) {
			return false;
		}
		
		if (obj is Money) {
			return Equals((Money)obj);
		}
		
		return false;
	}
	
	public bool Equals(Money other) {
		return this.Large == other.Large &&
			this.Small == other.Small;
	}
	
	public override int GetHashCode() {
		return Large.GetHashCode() ^ Small.GetHashCode();
	}
	
	public static bool operator ==(Money left, Money right) {
		return left.Equals(right);
	}
	
	public static bool operator !=(Money left, Money right) {
		return !left.Equals(right);
	}
	
	public static bool operator <(Money left, Money right) {
		if (left.Equals(right)) {
			return false;
		}
		
		if (left.Large < right.Large) {
			return true;
		}
		
		if (left.Large == right.Large) {
			if (left.Small < right.Small) {
				return true;
			}
			return false;
		}
		return false;
	}
	
	public static bool operator >(Money left, Money right) {
		return !(left.Equals(right) || left < right);
	}
	
	public static bool operator <=(Money left, Money right) {
		return left.Equals(right) || left < right;
	}
	
	public static bool operator >=(Money left, Money right) {
		return left.Equals(right) || left > right;
	}
	
	public static Money operator +(Money left, Money right) {  
		return new Money(left.Large + right.Large, left.Small + right.Small);
	}  
	
	public static Money FromSmall(int small) {
		return new Money(Math.Floor(small / 100f), (small / 100f) - Math.Floor(small / 100f));
	}
}