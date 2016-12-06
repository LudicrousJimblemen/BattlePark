using System;
using System.Collections.Generic;

namespace BattlePark {
	public class Park {
		public List<GridRegion> Regions = new List<GridRegion>();
	
		public long Owner;
		
		public string Name = "Battle Park";
		
		public Park(int x, int z, int width, int length, long owner) {
			this.Owner = owner;
			
			Regions.Add(new GridRegion(x, z, width, length));
		}
	}
}
