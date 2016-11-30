using System;
using System.Collections.Generic;

namespace BattlePark.Core {
	public class ServerUserUpdateNetMessage : NetMessage {
		public List<GameUser> Users { get; set; }
	}
}