using System;

namespace BattlePark.Core {
	public class ServerUserLeaveNetMessage : NetMessage {
		public GameUser User { get; set; }
	}
}