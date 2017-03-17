using System;

public struct Money {
	
	public int Value;

	private int Large {
		get {
			return (Value - Value % 100)/100;
		}
	}
	
	private int Small {
		get {
			return Value % 100;
		}
		/*
		get { return Small; }
		set {
			if (value >= 100) {
				Large += value / 100;
				Small = value % 100;
			} else if (value < 0) {
				Large += (value - 100) / 100;
				Small = 100 + (value % -100);
			}
		}
		*/
	}

	public Money(int large, int small) {
		Value = large * 100 + small;
	}

	public override bool Equals(object obj) {
		if (obj == null) {
			return false;
		}

		if (obj is Money) {
			return Equals((Money) obj);
		}

		return false;
	}

	public bool Equals(Money other) {
		return this.Value == other.Value;
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
		
		return left.Value < right.Value;
		/*

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
		*/
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
		return (Money)(left.Value + right.Value);
		//return new Money(left.Large + right.Large, left.Small + right.Small);
	}

	public static Money operator -(Money left, Money right) {
		return (Money)(left.Value - right.Value);
		//return new Money(left.Large - right.Large, left.Small - right.Small);
	}
	
	public override string ToString() {
		return ToString ("{0}.{1}");
	}
	public string ToString (string format, bool showSmall = true) {
		string large = Large.ToString ();
		string largeFinal = "";
		int rem = large.Length % 3;
		for (int i = 0; i < rem; i ++) {
			largeFinal += large[i];
		}
		if (rem != 0) {
			largeFinal += ",";
		}
		int groups = (int) large.Length / 3;
		for (int g = 0; g < groups; g ++) {
			if (g != 0) 
				largeFinal += ",";
			for (int k = 0; k < 3; k++) {
				largeFinal += large[rem + g * 3 + k];
			}
		}
		
		string smallString = Small.ToString ();
		if (Small < 10) {
			smallString = "0" + smallString;
		}
		if (showSmall) {
			return string.Format(format, largeFinal, smallString);
		} else {
			return string.Format(format, largeFinal);
		}
	}
	
	public string ToString(bool includeSmall) {
		return String.Format(LanguageManager.GetString(includeSmall? "game.gui.numericCurrencySmall" : "game.gui.numericCurrency"), Large, Small);
	}
	
	public static implicit operator Money(int small) {
		return new Money(small / 100, small % 100);
	}
	public static implicit operator long(Money money) {
		return money.Value;
	}
}