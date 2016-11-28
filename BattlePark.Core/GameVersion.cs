using System;

namespace BattlePark.Core {
	public struct GameVersion {
		public readonly int Major;
		public readonly int Minor;
		public readonly int Patch;
		
		public GameVersion(int major, int minor, int patch) {
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
		}
		
		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			
			if (obj is GameVersion) {
				return Equals((GameVersion)obj); // use Equals method below
			}
			
			return false;
		}
		
		public bool Equals(GameVersion other) {
			// add comparisions for all members here
			return
				this.Major == other.Major &&
			this.Minor == other.Minor &&
			this.Patch == other.Patch;
		}
		
		public override int GetHashCode() {
			// combine the hash codes of all members here (e.g. with XOR operator ^)
			return
				this.Major.GetHashCode() ^
			this.Minor.GetHashCode() ^
			this.Patch.GetHashCode();
		}
		
		public static bool operator ==(GameVersion left, GameVersion right) {
			return left.Equals(right);
		}
		
		public static bool operator !=(GameVersion left, GameVersion right) {
			return !left.Equals(right);
		}
		
		public static bool operator <(GameVersion left, GameVersion right) {
			if (left.Equals(right)) {
				return false;
			}
			
			if (left.Major < right.Major) {
				return true;
			}
			
			if (left.Major == right.Major) {
				if (left.Minor < right.Minor) {
					return true;
				}
				
				if (left.Minor == right.Minor) {
					if (left.Patch < right.Patch) {
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		
		public static bool operator >(GameVersion left, GameVersion right) {
			return !(left.Equals(right) || left < right);
		}
		
		public static bool operator <=(GameVersion left, GameVersion right) {
			return left.Equals(right) || left < right;
		}
		
		public static bool operator >=(GameVersion left, GameVersion right) {
			return left.Equals(right) || left > right;
		}
		
		public override string ToString() {
			return string.Format("v{0}.{1}.{2}", Major, Minor, Patch);
		}
	}
}
