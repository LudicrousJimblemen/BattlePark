using System;
using System.Collections.Generic;

namespace BattlePark.Core {
	public class ServerUserJoinNetMessage : NetMessage {
		public GameUser NewUser { get; set; }
	}
}