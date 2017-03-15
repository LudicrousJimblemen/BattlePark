using System;

public struct Money {
	public int Large { get; set; }

	private int small;
	public int Small {
		get { return small; }
		set {
			if (value >= 100) {
				Large += value / 100;
				small = value % 100;
			} else if (value < 0) {
				Large += (value - 100) / 100;
				small = 100 + (value % -100);
			}
		}
	}

	public Money(int large, int small) {
		this.Large = large;
		this.small = small; // Assign value.
		this.Small = small; // Run through sanity check.
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

	public static Money operator -(Money left, Money right) {
		return new Money(left.Large - right.Large, left.Small - right.Small);
	}
	
	public override string ToString() {
		return this.ToString(false);
	}
	
	public string ToString(bool includeSmall) {
		return String.Format(LanguageManager.GetString(includeSmall? "game.gui.numericCurrencySmall" : "game.gui.numericCurrency"), Large, Small);
	}
	
	public static implicit operator Money(int small) {
		return new Money(small / 100, small % 100);
	}
	
	public static implicit operator int(Money money) {
		return money.Large * 100 + money.Small;
	}
}