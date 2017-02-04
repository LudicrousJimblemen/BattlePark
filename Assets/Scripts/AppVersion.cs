using System;

namespace BattlePark.Core {
	public struct AppVersion {
		public int Major;
		public int Minor;
		public int Patch;
		
		public AppVersion(int major, int minor, int patch) {
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
		}
		
		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			
			if (obj is AppVersion) {
				return Equals((AppVersion)obj);
			}
			
			return false;
		}
		
		public bool Equals(AppVersion other) {
			return this.Major == other.Major &&
				this.Minor == other.Minor &&
				this.Patch == other.Patch;
		}
		
		public static bool operator ==(AppVersion left, AppVersion right) {
			return left.Equals(right);
		}
		
		public static bool operator !=(AppVersion left, AppVersion right) {
			return !left.Equals(right);
		}
		
		public static bool operator <(AppVersion left, AppVersion right) {
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
		
		public static bool operator >(AppVersion left, AppVersion right) {
			return !(left.Equals(right) || left < right);
		}
		
		public static bool operator <=(AppVersion left, AppVersion right) {
			return left.Equals(right) || left < right;
		}
		
		public static bool operator >=(AppVersion left, AppVersion right) {
			return left.Equals(right) || left > right;
		}
		
		public override string ToString() {
			return string.Format("v{0}.{1}.{2}", Major, Minor, Patch);
		}
	}
}
