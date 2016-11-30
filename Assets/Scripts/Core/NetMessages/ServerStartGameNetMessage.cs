using System;
using System.Collections.Generic;

namespace BattlePark.Core {
	public class ServerStartGameNetMessage : NetMessage {
		public List<long> Ids { get; set; }
		
		public int GridSize { get; set; }
	}
}